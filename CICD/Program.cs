using Microsoft.AspNetCore.Authentication.Certificate;
using Serilog;
using System.Security.Claims;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(SetAuthenticationOptions);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string[] urls = config.GetSection("urls").Get<string[]>();
builder.WebHost.UseUrls(urls);

builder.WebHost.UseSerilog();

// Add services to the container.
ConfigureBindings(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AnyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

static void ConfigureBindings(IServiceCollection services, ConfigurationManager configuration)
{
    string connectionString = configuration.GetConnectionString("defaultConnection");
    CICD.BLL.IoC.BindDataLayer(services, connectionString);

    IConfigurationSection configurationSection = configuration.GetSection("AppSettings");
    services.Configure<CICD.BO.AppSettings>(configurationSection);

    services.AddScoped<CICD.BLL.Interfaces.IConfiguration, CICD.BLL.Configuration>();
    services.AddScoped<CICD.BLL.Interfaces.IProcess, CICD.BLL.Process>();
    services.AddScoped<CICD.BLL.Interfaces.IDotnet, CICD.BLL.Dotnet>();
    services.AddScoped<CICD.BLL.Interfaces.IGit, CICD.BLL.Git>();
    services.AddScoped<CICD.BLL.Interfaces.IGitHub, CICD.BLL.GitHub>();
    services.AddScoped<CICD.BLL.Interfaces.IShell, CICD.BLL.Shell>();
    services.AddScoped<CICD.BLL.Validators.User>();
    services.AddScoped<CICD.BLL.Validators.Project>();
    services.AddScoped<CICD.BLL.Interfaces.IUser, CICD.BLL.User>();
    services.AddScoped<CICD.BLL.Interfaces.IProject, CICD.BLL.Project>();
    services.AddScoped<CICD.BLL.Interfaces.ICommit, CICD.BLL.Commit>();
    services.AddScoped<CICD.BLL.Interfaces.IBranch, CICD.BLL.Branch>();

    services.AddScoped<CICD.Mappers.IConfiguration, CICD.Mappers.Configuration>();
    services.AddScoped<CICD.Mappers.IUser, CICD.Mappers.User>();
    services.AddScoped<CICD.Mappers.IProject, CICD.Mappers.Project>();
    services.AddScoped<CICD.Mappers.ICommit, CICD.Mappers.Commit>();
    services.AddScoped<CICD.Mappers.IBranch, CICD.Mappers.Branch>();
    services.AddScoped<CICD.Mappers.IComment, CICD.Mappers.Comment>();
    services.AddScoped<CICD.Filters.ApiActionFilter>();
    services.AddScoped<CICD.Filters.ApiExceptionFilter>();

    services.AddCors(x => x.AddPolicy("AnyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    }));
}

static void SetAuthenticationOptions(CertificateAuthenticationOptions options)
{
    options.Events = new CertificateAuthenticationEvents
    {
        OnCertificateValidated = context =>
        {
            var claims = new[]
            {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer),
                    new Claim(
                        ClaimTypes.Name,
                        context.ClientCertificate.Subject,
                        ClaimValueTypes.String, context.Options.ClaimsIssuer)
                };

            context.Principal = new ClaimsPrincipal(
                new ClaimsIdentity(claims, context.Scheme.Name));
            context.Success();

            return Task.CompletedTask;
        }
    };
}