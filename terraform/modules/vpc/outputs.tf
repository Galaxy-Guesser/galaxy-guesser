output "subnet_id" {
    value = aws_subnet.default_subnet.id
}

output "second_subnet_id" {
    value = aws_subnet.second_subnet.id
}

output "vpc_id" {
    value = aws_vpc.galaxy_guesser_vpc.id
}