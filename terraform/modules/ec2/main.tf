# create private key
resource "tls_private_key" "instance_key" {
  algorithm = "RSA"
  rsa_bits  = 2048
}

# create key pair
resource "aws_key_pair" "instance_key_pair" {
  key_name   = var.key_name
  public_key = tls_private_key.instance_key.public_key_openssh

  tags = {
    Name = "instance-key-pair"
  }
}

# create instance
resource "aws_instance" "galaxy_guesser_api_instance" {
  ami           = var.ami_id
  instance_type = var.instance_type

  subnet_id                   = var.subnet_id
  vpc_security_group_ids      = [var.vpc_security_group_id]
  associate_public_ip_address = true

  key_name = aws_key_pair.instance_key_pair.key_name

  user_data = file("modules/ec2/data/instance_init_data.sh")

  tags = {
    Name = "galaxy-guesser-api-instance"
  }
}