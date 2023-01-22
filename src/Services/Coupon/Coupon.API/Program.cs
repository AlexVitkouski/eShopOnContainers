using Autofac.Extensions.DependencyInjection;
using Coupon.API;
using Coupon.API.Extensions;
using Coupon.API.Infrastructure;
using Coupon.API.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    CreateHostBuilder(args)
        .Build()
        //.SeedDatabaseStrategy<CouponContext>(context => new CouponSeed().SeedAsync(context).Wait())
        .SubscribersIntegrationEvents()
        .Run();
    
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureAppConfiguration((host, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            if (config.GetValue("UseVault", false))
            {
                builder.AddAzureKeyVault($"https://{config["Vault:Name"]}.vault.azure.net/", config["Vault:ClientId"], config["Vault:ClientSecret"]);
            }
        })
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseSerilog((host, builder) =>
        {
            builder.MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("ApplicationContext", host.HostingEnvironment.ApplicationName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(host.Configuration["Serilog:SeqServerUrl"]) ? "http://seq" : host.Configuration["Serilog:SeqServerUrl"])
                .WriteTo.Http(string.IsNullOrWhiteSpace(host.Configuration["Serilog:LogstashUrl"]) ? "http://logstash:8080" : host.Configuration["Serilog:LogstashUrl"])
                .ReadFrom.Configuration(host.Configuration);
        });

public partial class Program
{
    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}