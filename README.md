# MiniBlog
Practice Requirement
Fork this repository
Remove duplicate code in ArticleController and UserController
Make all test cases pass
Environment Requirement
.Net Core 6.0
Visual Studio
Reference:

https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-6.0
Tips:
Custom TestServer:
```C#
using DependencyInjectionSample.Interfaces;
using DependencyInjectionSample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddScoped<IMyDependency, MyDependency2>();

var app = builder.Build();

 ```
 
In Test:
```C#
    private HttpClient GetClient()
        {
            var factory = new WebApplicationFactory<Program>();
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => { services.AddScoped(service => _articleMocker.Object); });
            }).CreateClient();
        }
```
