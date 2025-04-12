output "vpc_security_group_id" {
    value = aws_security_group.db_security_group.id
}
