output "ec2_public_ip" {
  value = aws_instance.galaxy_guesser_api_instance.public_ip
}

output "ec2_endpoint" {
  value = aws_instance.galaxy_guesser_api_instance.public_dns
}

output "private_key" {
  value     = tls_private_key.instance_key.private_key_pem
  sensitive = true
}