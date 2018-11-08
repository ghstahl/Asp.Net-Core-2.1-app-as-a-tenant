# AspNetCoreApp-in-azure-function  
I am most likely commiting heresy here, but did you know that you can run the **[Microsoft.AspNetCore.TestHost.TestServer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-2.1 )** in an Azure function.

I feel like I am in that SouthPark episode where everything they tried to do that was clever was already done by the Simpsons.
In this case, AWS has already done it.

[Create a Serverless .NET Core 2.1 Web API with AWS Lambda](https://www.youtube.com/watch?v=OhEANj3Y6ZQ)  
So over in AWS this is a first class approach, whereas in Microsoft I have to bridge the gap in Functions using the TestServer.
Its basically the same process.  In the AWS Serverless WebApi app they introduced the following;
```
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
```
and
```
    /// <summary>
    /// This class extends from APIGatewayProxyFunction which contains the method FunctionHandlerAsync which is the 
    /// actual Lambda function entry point. The Lambda handler field should be set to
    /// 
    /// LambdaWebAppApi::LambdaWebAppApi.LambdaEntryPoint::FunctionHandlerAsync
    /// </summary>
    public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        /// <summary>
        /// The builder has configuration, logging and Amazon API Gateway already configured. The startup class
        /// needs to be configured in this method using the UseStartup<>() method.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IWebHostBuilder builder)
        {
            builder
                .UseStartup<Startup>();
        }
    }
```

This maps one to one with an Azure Function's entry point and the TestServer bridges over to the asp.net core 2.1 pipeline.

**So in the words of Butters, "AWS already did it!"**

# Postman  
[Get Postman](https://www.getpostman.com/)  
[Postman Collection](./AzureApi.postman_collection.json)  

# DNS using [xip.io](http://xip.io/)  
In postman you can have [environment settings](https://learning.getpostman.com/docs/postman/environments_and_globals/manage_environments/).  
I have an enviroment variable as follows;
```
domain=http://herb.127.0.0.1.xip.io:7071
```
The development function domain is allways;
```
http://localhost:7071
```
I always use [xip.io](http://xip.io/) so that I am assured that the code is agnostic of any changes like a loadbalancer in front of it.

# Support so far.
I don't expect to be hosting a webapp this way that serves up resources like *.css/*.js/etc.  If I were to host a HTML based website this way, all those resources would be on some CDN.  So in escense this would always be an api only way of hosting.

## GET (*)
## POST
### CONTENT-TYPE: application/x-www-form-urlencoded
### CONTENT-TYPE: application/json

# GraphQL client [Altair](https://altair.sirmuel.design)

## GraphQL endpoint
```
http://herb.127.0.0.1.xip.io:7071/api/GraphQL
```
## Query
```
query{
  hero{
    appearsIn
    id
    name
  }
}
```
## Result 
```
{
  "data": {
    "hero": {
      "appearsIn": [
        "NEWHOPE",
        "EMPIRE",
        "JEDI"
      ],
      "id": "3",
      "name": "R2-D2"
    }
  }
}
```




