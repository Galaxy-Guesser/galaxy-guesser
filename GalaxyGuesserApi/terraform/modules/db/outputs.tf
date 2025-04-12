output "galaxy_guesser_db_endpoint" {
  value = aws_db_instance.galaxy_guesser_db_instance.endpoint
}

output "galaxy_guesser_db_address" {
  value = aws_db_instance.galaxy_guesser_db_instance.address
}

output "galaxy_guesser_db_port" {
  value = aws_db_instance.galaxy_guesser_db_instance.port
}

output "galaxy_guesser_db_name" {
  value     = var.db_name
  sensitive = true
}

output "galaxy_guesser_db_user" {
  value     = aws_db_instance.galaxy_guesser_db_instance.username
  sensitive = true
}

output "galaxy_guesser_db_password" {
  value     = aws_db_instance.galaxy_guesser_db_instance.password
  sensitive = true
}
