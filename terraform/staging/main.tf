provider "aws" {
    region  = "eu-west-2"
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

# SSM Parameters - Systems Manager/Parameter Store
data "aws_ssm_parameter" "housing_finance_uhservicesystemcredentials_username" {
    name = "/housing-finance/staging/uhservicesystemcredentials-username"
}
data "aws_ssm_parameter" "housing_finance_uhserviceusercredentials_username" {
    name = "/housing-finance/staging/uhserviceusercredentials-username"
}
data "aws_ssm_parameter" "housing_finance_uhserviceusercredentials_userpassword" {
    name = "/housing-finance/staging/uhserviceusercredentials-userpassword"
}
data "aws_ssm_parameter" "housing_finance_dynamics365settings_aadinstance" {
    name = "/housing-finance/staging/dynamics365settings-aadinstance"
}
data "aws_ssm_parameter" "housing_finance_dynamics365settings_appkey" {
    name = "/housing-finance/staging/dynamics365settings-appkey"
}
data "aws_ssm_parameter" "housing_finance_dynamics365settings_clientid" {
    name = "/housing-finance/staging/dynamics365settings-clientid"
}
data "aws_ssm_parameter" "housing_finance_dynamics365settings_organizationurl" {
    name = "/housing-finance/staging/dynamics365settings-organizationurl"
}
data "aws_ssm_parameter" "housing_finance_dynamics365settings_tenantid" {
    name = "/housing-finance/staging/dynamics365settings-tenantid"
}
data "aws_ssm_parameter" "housing_finance_sentrysettings_environment" {
    name = "/housing-finance/staging/sentrysettings-environment"
}
data "aws_ssm_parameter" "housing_finance_sentrysettings_url" {
    name = "/housing-finance/staging/sentrysettings-url"
}
data "aws_ssm_parameter" "housing_finance_servicesettings_agreementserviceendpoint" {
    name = "/housing-finance/staging/servicesettings-agreementserviceendpoint"
}
data "aws_ssm_parameter" "housing_finance_uh_url" {
    name = "/housing-finance/staging/uh-url"
}

terraform {
    backend "s3" {
        bucket  = "terraform-state-housing-staging"
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
        subnets          = ["subnet-0743d86e9b362fa38", "subnet-0ea0020a44b98a2ca"]
        security_groups = ["sg-00c197e980177983d"]
        assign_public_ip = false
    }
    desired_count = 1
    load_balancer {
        target_group_arn = aws_lb_target_group.lb_tg.arn
        container_name   = "tenancy-api-container"
        container_port   = 80
    }
}

resource "aws_ecs_task_definition" "tenancy-api-ecs-task-definition" {
    family                   = "ecs-task-definition-tenancy-api"
    network_mode             = "awsvpc"
    requires_compatibilities = ["FARGATE"]
    memory                   = "1024"
    cpu                      = "512"
    execution_role_arn       = "arn:aws:iam::087586271961:role/ecsTaskExecutionRole"
    container_definitions    = <<DEFINITION
[
  {
    "name": "tenancy-api-container",
    "image": "364864573329.dkr.ecr.eu-west-2.amazonaws.com/hackney/apps/tenancy-api:${var.sha1}",
    "memory": 1024,
    "cpu": 512,
    "essential": true,
    "portMappings": [
      {
        "containerPort": 80
      }
    ],
    "logConfiguration": {
        "logDriver": "awslogs",
        "options": {
            "awslogs-group": "ecs-task-definition-tenancy-api",
            "awslogs-region": "eu-west-2",
            "awslogs-stream-prefix": "tenancy-api-logs"
        }
    },
    "environment": [
      {
        "name": "UH_DATABASE_URL",
        "value": "${data.aws_ssm_parameter.housing_finance_uh_url.value}"
      },
      {
        "name": "Credentials__UHServiceSystemCredentials__UserName",
        "value": "${data.aws_ssm_parameter.housing_finance_uhservicesystemcredentials_username.value}"
      },
      {
        "name": "Credentials__UHServiceUserCredentials__UserName",
        "value": "${data.aws_ssm_parameter.housing_finance_uhserviceusercredentials_username.value}"
      },
      {
        "name": "Credentials__UHServiceUserCredentials__UserPassword",
        "value": "${data.aws_ssm_parameter.housing_finance_uhserviceusercredentials_userpassword.value}"
      },
      {
        "name": "Dynamics365Settings__AadInstance",
        "value": "${data.aws_ssm_parameter.housing_finance_dynamics365settings_aadinstance.value}"
      },
      {
        "name": "Dynamics365Settings__AppKey",
        "value": "${data.aws_ssm_parameter.housing_finance_dynamics365settings_appkey.value}"
      },
      {
        "name": "Dynamics365Settings__ClientId",
        "value": "${data.aws_ssm_parameter.housing_finance_dynamics365settings_clientid.value}"
      },
      {
        "name": "Dynamics365Settings__OrganizationUrl",
        "value": "${data.aws_ssm_parameter.housing_finance_dynamics365settings_organizationurl.value}"
      },
      {
        "name": "Dynamics365Settings__TenantId",
        "value": "${data.aws_ssm_parameter.housing_finance_dynamics365settings_tenantid.value}"
      },
      {
        "name": "SentrySettings__Url",
        "value": "${data.aws_ssm_parameter.housing_finance_sentrysettings_url.value}"
      },
      {
        "name": "SentrySettings__Environment",
        "value": "${data.aws_ssm_parameter.housing_finance_sentrysettings_environment.value}"
      },
      {
        "name": "ServiceSettings__AgreementServiceEndpoint",
        "value": "${data.aws_ssm_parameter.housing_finance_servicesettings_agreementserviceendpoint.value}"
      },
      {
        "name": "UH_URL",
        "value": "${data.aws_ssm_parameter.housing_finance_uh_url.value}"
      }
    ]
  }
]
DEFINITION
}

# Network Load Balancer (NLB) setup

resource "aws_lb" "lb" {
    name               = "lb-tenancy-api"
    internal           = true
    load_balancer_type = "network"
    subnets            = ["subnet-0743d86e9b362fa38", "subnet-0ea0020a44b98a2ca"]// Get this from AWS (data)
    enable_deletion_protection = false
    tags = {
        Environment = "staging"
    }
}
resource "aws_lb_target_group" "lb_tg" {
    depends_on  = [
        aws_lb.lb
    ]
    name_prefix = "ma-tg-"
    port        = 80
    protocol    = "TCP"
    vpc_id      = "vpc-064521a7a4109ba31" // Get this from AWS (data)
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
    port              = 80
    protocol    = "TCP"
    default_action {
        target_group_arn = aws_lb_target_group.lb_tg.id
        type             = "forward"
    }
}

# API Gateway setup

# VPC Link
resource "aws_api_gateway_vpc_link" "this" {
    name = "vpc-link-tenancy-api"
    target_arns = [aws_lb.lb.arn]
}
# API Gateway, Private Integration with VPC Link
# and deployment of a single resource that will take ANY
# HTTP method and proxy the request to the NLB
resource "aws_api_gateway_rest_api" "main" {
    name = "staging-tenancy-api"
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
    uri                     = "http://${aws_lb.lb.dns_name}:80/{proxy}"
    integration_http_method = "ANY"
    connection_type = "VPC_LINK"
    connection_id   = aws_api_gateway_vpc_link.this.id
}
resource "aws_api_gateway_deployment" "main" {
    rest_api_id = aws_api_gateway_rest_api.main.id
    stage_name = "staging"
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


resource "aws_api_gateway_usage_plan" "main" {
    name = "tenancy_api_staging_usage_plan"

    api_stages {
        api_id = aws_api_gateway_rest_api.main.id
        stage  = aws_api_gateway_deployment.main.stage_name
    }
}

resource "aws_api_gateway_api_key" "main" {
    name = "tenancy_api_staging_key"
}

resource "aws_api_gateway_usage_plan_key" "main" {
    key_id        = aws_api_gateway_api_key.main.id
    key_type      = "API_KEY"
    usage_plan_id = aws_api_gateway_usage_plan.main.id
}
