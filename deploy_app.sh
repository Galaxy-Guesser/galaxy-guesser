#install terraform

sudo apt-add-repository --yes "deb [arch=$(dpkg --print-architecture)] https://apt.releases.hashicorp.com $(lsb_release -cs) main"
sudo apt update
sudo apt install terraform

echo "Running Terraform..."

cd terraform
echo "Running Terraform init..."
terraform init -reconfigure
echo "Done Running Terraform init..."
echo "Running terraform plan..."
terraform plan
echo "Done running terraform plan"