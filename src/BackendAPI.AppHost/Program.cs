using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;


var builder = DistributedApplication.CreateBuilder(args);

var userApi = builder.AddProject<Projects.User_API>("user-api");

var app = builder.Build();


/* if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
} */


app.Run();
