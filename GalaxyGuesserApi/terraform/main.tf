terraform {
    required_providers {
        aws = {
            source = "hashicorp/aws"
        }
    }
}

provider "aws"{
    region = var.aws_region
    shared_credentials_files = ["~/.aws/credentials"]
    profile = "guess_what"
}

module "vpc" {
    source = "./modules/vpc"

}

module "db" {
    source = "./modules/db"

    subnet_id = module.vpc.subnet_id
    second_subnet_id = module.vpc.second_subnet_id
    vpc_security_group_id = module.security-group.vpc_security_group_id
}

module "ec2" {
    source = "./modules/ec2"

    subnet_id = module.vpc.subnet_id
    second_subnet_id = module.vpc.second_subnet_id
    vpc_security_group_id = module.security-group.vpc_security_group_id
}

module "s3" {
    source = "./modules/s3"
}

module "security-group" {
    source = "./modules/security-group"

    vpc_id = module.vpc.vpc_id
}