resource "aws_s3_bucket" "tfer--iac-002D-tst" {
  arn            = "arn:aws:s3:::iac-tst"
  bucket         = "iac-tst"
  force_destroy  = "false"
  hosted_zone_id = "Z3AQBSTGFYJSTF"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Resource": "arn:aws:s3:::iac-tst/*",
      "Sid": "PublicReadGetObject"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  tags = {
    iac-tst = ""
  }

  tags_all = {
    iac-tst = ""
  }

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    index_document = "index.html"
  }

  website_domain   = "s3-website-us-east-1.amazonaws.com"
  website_endpoint = "iac-tst.s3-website-us-east-1.amazonaws.com"
}
