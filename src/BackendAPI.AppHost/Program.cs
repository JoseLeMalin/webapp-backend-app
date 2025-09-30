using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Npgsql;


var builder = DistributedApplication.CreateBuilder(args);

var basketApi = builder.AddProject<Projects.User_API>("user-api");

builder.Build().Run();
