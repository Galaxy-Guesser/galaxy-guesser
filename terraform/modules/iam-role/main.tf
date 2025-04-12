resource "aws_iam_openid_connect_provider" "github" {
    url = "https://token.actions.githubusercontent.com"

    client_id_list = ["sts.amazonaws.com"]

    thumbprint_list = ["6938fd4d98bab03faadb97b34396831e3780aea1"] # GitHub's cert thumbprint
}

resource "aws_iam_role" "github_actions_role" {
    name = "github-actions-deploy"

    assume_role_policy = jsonencode({
        Version = "2012-10-17",
        Statement = [{
            Effect = "Allow",
            Principal = {
                Federated = aws_iam_openid_connect_provider.github.arn
            },
            Action = "sts:AssumeRoleWithWebIdentity",
            Condition = {
                StringEquals = {
                "token.actions.githubusercontent.com:aud" = "sts.amazonaws.com"
                #Limit to specific repo/branch
                # "token.actions.githubusercontent.com:sub" = "repo:Galaxy-Guesser/galaxy-guesser:refs/head/main"
                }
                StringLike = {
                  "token.actions.githubusercontent.com:sub" = "repo:Galaxy-Guesser/galaxy-guesser:ref:refs/head/vusi/terraform"
                }
            }
        }]
    })
}

resource "aws_iam_role_policy_attachment" "example_attach" {
    role = aws_iam_role.github_actions_role.name
    policy_arn = "arn:aws:iam::aws:policy/PowerUserAccess"
}