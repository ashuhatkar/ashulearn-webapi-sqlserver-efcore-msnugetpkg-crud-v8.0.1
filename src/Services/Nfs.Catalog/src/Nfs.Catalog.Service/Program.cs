/*--****************************************************************************
  --* Project Name    : Nfs.Services
  --* Reference       : Autofac.Extensions.DependencyInjection
  --*                   Microsoft.AspNetCore.Builder
  --*                   Microsoft.Extensions.Configuration
  --*                   Microsoft.Extensions.DependencyInjection
  --*                   Microsoft.Extensions.Hosting
  --*                   Nfs.Core.Configuration
  --*                   Nfs.Core.Infrastructure
  --*                   Nfs.Data
  --*                   Nfs.Services.Installation
  --*                   Nfs.Web.Framework.Infrastructure.Extensions
  --* Description     : Startup
  --* Configuration Record
  --* Review            Ver  Author           Date      Cr       Comments
  --* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
  --****************************************************************************/
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nfs.Core.Configuration;
using Nfs.Core.Infrastructure;
using Nfs.Data;
using Nfs.Services.Installation;
using Nfs.Web.Framework.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(NfsConfigurationDefaults.AppSettingsFilePath, true, true);
if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
{
    var path = string.Format(NfsConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
    builder.Configuration.AddJsonFile(path, true, true);
}
builder.Configuration.AddEnvironmentVariables();

//load application settings
builder.Services.ConfigureApplicationSettings(builder);

var appSettings = Singleton<AppSettings>.Instance;
var useAutofac = appSettings.Get<CommonConfig>().UseAutofac;

if (useAutofac)
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
else
    builder.Host.UseDefaultServiceProvider(options =>
    {
        //we don't validate the scopes, since at the app start and the initial configuration we need 
        //to resolve some services (registered as "scoped") through the root container
        options.ValidateScopes = false;
        options.ValidateOnBuild = true;
    });

//add services to the application and configure service provider
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//configure the application HTTP request pipeline
app.ConfigureRequestPipeline();
app.StartEngine();

app.MapControllers();

//create db and seed data
if (!DataSettingsManager.IsDatabaseInstalled())
    InstallationExtensions.createAndSeedData();

app.Run();