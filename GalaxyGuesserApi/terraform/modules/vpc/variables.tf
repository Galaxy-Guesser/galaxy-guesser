variable "vpc_cidr_block" {
    type = string
    default = "10.0.0.0/16"
}

variable "subnet_cidr_block" {
    type = string
    default = "10.0.1.0/24"
}

variable "second_subnet_cidr_block" {
    type = string
    default = "10.0.2.0/24"
}

variable "availability_zone" {
  type    = string
  default = "af-south-1a"
}

variable "availability_zone_b" {
  type    = string
  default = "af-south-1b"
}
