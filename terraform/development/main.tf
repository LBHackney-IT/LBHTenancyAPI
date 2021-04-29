provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
    application_name = "lbh tenancy api"
    parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

data "aws_ecs_cluster" "ecs_cluster_for_manage_arrears" {
    cluster_name = "ecs-cluster-for-manage-arrears"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-housing-development"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/lbh-tenancy-api/state"
  }
}

#Elastic Container Registry (ECR) setup

resource "aws_ecr_repository" "tenancy-api" {
    name                 = "hackney/apps/tenancy-api"
    image_tag_mutability = "MUTABLE"
}

resource "aws_ecr_repository_policy" "tenancy-api-policy" {
    repository = aws_ecr_repository.tenancy-api.name
    policy     = <<EOF
  {
    "Version": "2008-10-17",
    "Statement": [
      {
        "Sid": "adds full ecr access to the repository",
        "Effect": "Allow",
        "Principal": "*",
        "Action": [
          "ecr:BatchCheckLayerAvailability",
          "ecr:BatchGetImage",
          "ecr:CompleteLayerUpload",
          "ecr:GetDownloadUrlForLayer",
          "ecr:GetLifecyclePolicy",
          "ecr:InitiateLayerUpload",
          "ecr:PutImage",
          "ecr:UploadLayerPart",
          "logs:CreateLogGroup"
        ]
      }
    ]
  }
  EOF
}

# Elastic Container Service (ECS) setup

resource "aws_ecs_service" "tenancy-api-ecs-service" {
    name            = "tenancy-api-ecs-service"
    cluster         = data.aws_ecs_cluster.ecs_cluster_for_manage_arrears.id
    task_definition = aws_ecs_task_definition.tenancy-api-ecs-task-definition.arn
    launch_type     = "FARGATE"
    network_configuration {
        subnets          = ["subnet-0140d06fb84fdb547", "subnet-05ce390ba88c42bfd"]
        security_groups = ["sg-00d2e14f38245dd0b"]
        assign_public_ip = false
    }
    desired_count = 1
    load_balancer {
        target_group_arn = aws_lb_target_group.lb_tg.arn
        container_name   = "${var.app_name}-container"
        container_port   = var.app_port
    }
}

resource "aws_ecs_task_definition" "tenancy-api-ecs-task-definition" {
    family                   = "ecs-task-definition-${var.app_name}"
    network_mode             = "awsvpc"
    requires_compatibilities = ["FARGATE"]
    memory                   = "4096"
    cpu                      = "512"
    execution_role_arn       = "arn:aws:iam::364864573329:role/ecsTaskExecutionRole"
    container_definitions    = <<DEFINITION
[
  {
    "name": "${var.app_name}-container",
    "image": "364864573329.dkr.ecr.eu-west-2.amazonaws.com/hackney/apps/${var.app_name}:latest",
    "memory": 4096,
    "cpu": 512,
    "essential": true,
    "portMappings": [
      {
        "containerPort": ${var.app_port}
      }
    ],
    "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
            "awslogs-group": "ecs-task-definition-${var.app_name}",
            "awslogs-region": "eu-west-2",
            "awslogs-stream-prefix": "${var.app_name}-logs"
        }
    },
    "environment": [
      {
        "name": "UH_DATABASE_URL",
        "value": "HackneyAPIIncomeCollection"
      }
    ]
  }
]
DEFINITION
}

# Network Load Balancer (NLB) setup

resource "aws_lb" "lb" {
    name               = "lb-${var.app_name}"
    internal           = true
    load_balancer_type = "network"
    subnets            = ["subnet-0140d06fb84fdb547", "subnet-05ce390ba88c42bfd"]// Get this from AWS (data)
    enable_deletion_protection = false
    tags = {
        Environment = var.environment_name
    }
}
resource "aws_lb_target_group" "lb_tg" {
    depends_on  = [
        aws_lb.lb
    ]
    name_prefix = "ma-tg-"
    port        = var.app_port
    protocol    = "TCP"
    vpc_id      = "vpc-0d15f152935c8716f" // Get this from AWS (data)
    target_type = "ip"
    stickiness {
        enabled = false
        type = "lb_cookie"
    }
    lifecycle {
        create_before_destroy = true
    }
}
# Redirect all traffic from the NLB to the target group
resource "aws_lb_listener" "lb_listener" {
    load_balancer_arn = aws_lb.lb.id
    port              = var.app_port
    protocol    = "TCP"
    default_action {
        target_group_arn = aws_lb_target_group.lb_tg.id
        type             = "forward"
    }
}

# API Gateway setup

# VPC Link
resource "aws_api_gateway_vpc_link" "this" {
    name = "vpc-link-${var.app_name}"
    target_arns = [aws_lb.lb.arn]
}
# API Gateway, Private Integration with VPC Link
# and deployment of a single resource that will take ANY
# HTTP method and proxy the request to the NLB
resource "aws_api_gateway_rest_api" "main" {
    name = "${var.environment_name}-${var.app_name}"
}
resource "aws_api_gateway_resource" "main" {
    rest_api_id = aws_api_gateway_rest_api.main.id
    parent_id   = aws_api_gateway_rest_api.main.root_resource_id
    path_part   = "{proxy+}"
}
resource "aws_api_gateway_method" "main" {
    rest_api_id   = aws_api_gateway_rest_api.main.id
    resource_id   = aws_api_gateway_resource.main.id
    http_method   = "ANY"
    authorization = "NONE"
    request_parameters = {
        "method.request.path.proxy" = true
        "method.request.header.Authorization" = false
    }
}
resource "aws_api_gateway_integration" "main" {
    rest_api_id = aws_api_gateway_rest_api.main.id
    resource_id = aws_api_gateway_resource.main.id
    http_method = aws_api_gateway_method.main.http_method
    request_parameters = {
        "integration.request.path.proxy" = "method.request.path.proxy"
    }
    type                    = "HTTP_PROXY"
    uri                     = "http://${aws_lb.lb.dns_name}:${var.app_port}/{proxy}"
    integration_http_method = "ANY"
    connection_type = "VPC_LINK"
    connection_id   = aws_api_gateway_vpc_link.this.id
}
resource "aws_api_gateway_deployment" "main" {
    rest_api_id = aws_api_gateway_rest_api.main.id
    stage_name = var.environment_name
    depends_on = [aws_api_gateway_integration.main]
    variables = {
        # just to trigger redeploy on resource changes
        resources = join(", ", [aws_api_gateway_resource.main.id])
        # note: redeployment might be required with other gateway changes.
        # when necessary run `terraform taint <this resource's address>`
    }
    lifecycle {
        create_before_destroy = true
    }
}
