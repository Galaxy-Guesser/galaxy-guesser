resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "aws_subnet_group_db"
  subnet_ids = [var.subnet_id, var.second_subnet_id]
}

resource "aws_db_instance" "galaxy_guesser_db_instance" {
  identifier             = "galaxy-guesser-db-instance"
  instance_class         = "db.t3.micro"
  allocated_storage      = 5
  engine                 = "postgres"
  engine_version         = "17.2"
  skip_final_snapshot    = true
  publicly_accessible    = true
  vpc_security_group_ids = [var.vpc_security_group_id]
  db_subnet_group_name   = aws_db_subnet_group.db_subnet_group.name
  username               = var.db_username
  password               = var.db_password
}