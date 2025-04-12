variable "subnet_id" {
    type = string
}

variable "second_subnet_id" {
    type = string
}

variable "db_username" {
    type = string
    default = "user1"
}

variable "db_password" {
    type = string
    sensitive = true
    default = "password1234"
}

variable "db_name" {
    type = string
    sensitive = true
    default = "galaxy_guesser_db"
}

variable "vpc_security_group_id" {
    type = string
}
