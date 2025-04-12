output "github_actions_role_arn" {
  value = module.iam-role.github_actions_role_arn
}

output "github_actions_role" {
    value = module.iam-role.github_actions_role
}