variable "ami_id" {
    type = string
    default = "ami-080b2e8ce472c5091"
}

variable "instance_type" {
    type = string
    default = "t3.micro"
}

variable "key_name" {
    type = string
    default = "galaxy_guesser_key"
}

variable "subnet_id" {
    type = string
}

variable "second_subnet_id" {
    type = string
}

variable "vpc_security_group_id" {
    type = string
}