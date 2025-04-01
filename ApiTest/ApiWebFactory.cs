using Application.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ApiTest;

public class ApiWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //Nothing is mock yet, but if it's needed, it can be done here
    }
}
