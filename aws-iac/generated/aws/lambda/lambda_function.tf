resource "aws_lambda_function" "tfer--iac-002D-tst" {
  function_name                  = "iac-tst"
  handler                        = "net-api::net_api.LambdaEntryPoint::FunctionHandlerAsync"
  memory_size                    = "512"
  package_type                   = "Zip"
  reserved_concurrent_executions = "-1"
  role                           = "arn:aws:iam::758543808647:role/service-role/iac-tst-role-frc8mzop"
  runtime                        = "dotnetcore3.1"
  source_code_hash               = "LkG4FXDSABnLetxCy1405/Rg26vie4w185GWfSKq2qE="

  tags = {
    iac-tst = ""
  }

  tags_all = {
    iac-tst = ""
  }

  timeout = "15"

  tracing_config {
    mode = "PassThrough"
  }
}
