# AspNetCoreApp-in-azure-function  
I am most likely commiting heresy here, but did you know that you can run the **[Microsoft.AspNetCore.TestHost.TestServer](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.testhost.testserver?view=aspnetcore-2.1 )** in an Azure function.

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




