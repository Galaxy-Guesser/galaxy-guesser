# create security group for ssh and api end-point
resource "aws_security_group" "instance_security_group" {
#   vpc_id = module.vpc.aws_vpc.galaxy_guesser_vpc.id
  vpc_id = var.vpc_id
  name   = "galaxy-guesser-instance-security-group"

  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  ingress {
    from_port   = var.api_port
    to_port     = var.api_port
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "instance-security-group"
  }
}

resource "aws_security_group" "db_security_group" {
#   vpc_id      = module.vpc.aws_vpc.galaxy_guesser_vpc.id
  vpc_id = var.vpc_id
  name        = "db_security_group"
  ingress {
    from_port   = var.db_port
    to_port     = var.db_port
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}