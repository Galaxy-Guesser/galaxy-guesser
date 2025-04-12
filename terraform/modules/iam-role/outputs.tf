output "github_actions_role_arn" {
    value = aws_iam_role.github_actions_role.arn
}

output "github_actions_role" {
    value = aws_iam_openid_connect_provider.github.arn
}