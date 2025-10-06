using BackendAPI.User.API.Data;

public static class Extensions
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        // Avoid loading full database config and migrations if startup
        // is being invoked from build-time OpenAPI generation
        if (builder.Environment.IsDevelopment())
        {
            var conString = builder.Configuration["ApplicationDbContext"];
            builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
    opt.UseNpgsql(conString)
        .UseSnakeCaseNamingConvention()
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
    );
            // builder.Services.AddDbContext<ApplicationDbContext>();
            return;
        }
    }
}
