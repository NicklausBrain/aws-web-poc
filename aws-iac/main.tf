terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "3.42.0"
    }
  }
}

provider "aws" {
  region     = "us-east-1"
  access_key = "xxx"
  secret_key = "xxx"
}
