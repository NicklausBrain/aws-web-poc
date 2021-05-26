using Amazon.CDK;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;

namespace AwsIacDotnet
{
    public class AwsIacDotnetStack : Stack
    {
        internal AwsIacDotnetStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            #region Identity

            var userPool = new UserPool(this, "iac-tst2-user-pool", new UserPoolProps
            {
                UserPoolName = "iac-tst2-user-pool",
                SignInCaseSensitive = false,
                SignInAliases = new SignInAliases
                {
                    Email = true,
                }
            });

            var client = userPool.AddClient("iac-tst2-client", new UserPoolClientOptions
            {
                AuthFlows = new AuthFlow
                {
                    UserPassword = true,
                    AdminUserPassword = true,
                },
                SupportedIdentityProviders = new[] { UserPoolClientIdentityProvider.COGNITO },
            });

            var clientId = client.UserPoolClientId; // to be used as env variable for lambda (?)

            #endregion

            // Web site s3 bucket
            var webSiteS3 = new Bucket(this, "iac-tst2-s3", new BucketProps
            {
                BucketName = "iac-tst2-s3",
                Versioned = false,
                PublicReadAccess = true,
                WebsiteIndexDocument = "index.html",
                WebsiteErrorDocument = "error.html",
            });

            // Dotnet API lambda
            var apiLambda = new Function(this, "iac-tst2-lambda", new FunctionProps
            {
                FunctionName = "iac-tst2-lambda",
                Runtime = Runtime.DOTNET_CORE_3_1, // execution environment
                Code = Code.FromAsset(@"C:\Projects\aws-web-poc\net-api\net-api\bin\Release\netcoreapp3.1\net-api.zip"), // Code loaded from the "lambda" directory
                Handler = "net-api::net_api.LambdaEntryPoint::FunctionHandlerAsync", // lambda handler id
                MemorySize = 512,
            });

            #region API Gateway Config

            var backendIntegration = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = apiLambda,
            });

            var frontendIntegration = new HttpProxyIntegration(new HttpProxyIntegrationProps
            {
                Method = HttpMethod.GET,
                Url = webSiteS3.BucketWebsiteUrl,
            });

            var httpApi = new HttpApi(this, "iac-tst2-gate", new HttpApiProps
            {
                ApiName = "iac-tst2-gate",
                DefaultIntegration = frontendIntegration,
            });

            httpApi.AddRoutes(new AddRoutesOptions
            {
                Path = "/api/{proxy+}",
                Methods = new[] { HttpMethod.ANY },
                Integration = backendIntegration
            });

            httpApi.AddRoutes(new AddRoutesOptions
            {
                Path = "/swagger/{proxy+}",
                Methods = new[] { HttpMethod.GET },
                Integration = backendIntegration
            });

            #endregion API Gateway Config

            #region Storages

            // Media content s3 bucket
            //var mediaContentS3 = new Bucket(this, "iac-tst2-s3", new BucketProps
            //{
            //    BucketName = "iac-tst2-s3",
            //    Versioned = false,
            //    PublicReadAccess = true,
            //    WebsiteIndexDocument = "index.html",
            //    WebsiteErrorDocument = "error.html",
            //});

            // Example automatically generated without compilation. See https://github.com/aws/jsii/issues/826
            //var cluster = new DatabaseCluster(this, "iac-tst2-doc-db", new DatabaseClusterProps
            //{
            //    InstanceType = InstanceType.Of(InstanceClass.STANDARD3, InstanceSize.SMALL),
            //    MasterUser = new Login
            //    {
            //        Username = "sa",
            //        // Password = new SecretValue("Password12"),
            //    },
            //    Vpc = new Vpc(this, "iac-tst2-doc-db-vpc", new VpcProps
            //    {
            //        Cidr = "10.0.0.0/16",
            //    }),
            //    VpcSubnets = new SubnetSelection
            //    {
            //        SubnetType = SubnetType.PUBLIC,
            //    }
            //});


            #endregion Storages
        }
    }
}
