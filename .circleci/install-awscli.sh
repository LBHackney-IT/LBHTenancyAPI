#!/bin/bash

apt-get update
apt-get install -y awscli

cat > ~/.aws/credentials << EOF
[default]
aws_access_key_id=$AWS_ACCESS_KEY_ID
aws_secret_access_key=$AWS_SECRET_ACCESS_KEY
EOF

cat > ~/.aws/config << EOF
[default]
region=$AWS_REGION
EOF
