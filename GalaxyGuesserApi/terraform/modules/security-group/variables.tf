variable "vpc_id" {
    type = string
}

variable "api_port" {
    type = number
    default = 8080
}

variable "db_port" {
    type = number
    default = 5432
}