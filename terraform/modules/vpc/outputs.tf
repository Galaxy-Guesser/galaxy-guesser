output "vpc_id" {
  value = aws_vpc.main.id
}

output "public_subnet_ids" {
  value = [for subnet in aws_subnet.public : subnet.id]
}

# output "internet_gateway_id" {
#   value = aws_internet_gateway.this.id
# }

# output "sg_id" {
#   value = aws_security_group.this.id
# }

# output "vpc_security_group_ids" {
#   value = [aws_security_group.this.id]
# }

# output "subnet_id" {
#   value = [aws_subnet.this[*].id]
# }