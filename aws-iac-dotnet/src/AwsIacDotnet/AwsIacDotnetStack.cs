using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.APIGatewayv2;
using Amazon.CDK.AWS.APIGatewayv2.Integrations;

namespace AwsIacDotnet
{
    public class AwsIacDotnetStack : Stack
    {
        internal AwsIacDotnetStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here

            var s3WebSite = new Bucket(this, "iac-tst2-s3", new BucketProps
            {
                Versioned = true,
                PublicReadAccess = true,
                WebsiteIndexDocument = "index.html",
            });

            // Defines a new lambda resource
            var backendApp = new Function(this, "iac-tst2-lambda", new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1, // execution environment
                Code = Code.FromAsset(@"C:\Projects\aws-web-poc\net-api\net-api\bin\Release\netcoreapp3.1\net-api.zip"), // Code loaded from the "lambda" directory
                Handler = "net-api::net_api.LambdaEntryPoint::FunctionHandlerAsync", // lambda handler id
                MemorySize = 512,
            });

            var backendIntegration = new LambdaProxyIntegration(new LambdaProxyIntegrationProps
            {
                Handler = backendApp,
            });

            var frontendIntegration = new HttpProxyIntegration(new HttpProxyIntegrationProps
            {
                Method = HttpMethod.GET,
                Url = s3WebSite.BucketWebsiteUrl,
            });

            // defines an API Gateway REST API resource backed by our function.
            var httpApi = new HttpApi(this, "web-site", new HttpApiProps
            {
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
        }
    }
}
