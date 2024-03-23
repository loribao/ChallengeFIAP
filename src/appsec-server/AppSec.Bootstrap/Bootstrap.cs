using AppSec.Domain;
using AppSec.Domain.Commands;
using AppSec.Domain.Commands.Base;
using AppSec.Domain.Interfaces.ICommands;
using AppSec.Domain.Interfaces.IDrivers;
using AppSec.Domain.Interfaces.IRepository;
using AppSec.Infra;
using AppSec.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppSec.Bootstrap;

public static class Register
{

    public static IServiceCollection Add(IServiceCollection Services, IConfiguration configuration)
    {
        var paths = configuration.GetRequiredSection("paths");
        Services.AddSingleton<ContextAppSec>(options =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ContextAppSec>();
            var paths = configuration.GetRequiredSection("paths");
            optionsBuilder.UseSqlite($"Data Source={GetDatabasePath(paths.GetSection("database").Value ?? "appsec.sqlite")}", b =>
            {
                b.MigrationsAssembly("AppSec.Infra");
            });
            return new ContextAppSec(optionsBuilder.Options);
        });
        Services.AddScoped<ILanguageDriverSast, LanguageDriverSast>();
        Services.AddScoped<IProjectRepository, ProjectRepository>();
        Services.AddScoped<IGitRepository, GitRepository>();
        Services.AddScoped<IDastRepository, DastRepository>();
        Services.AddScoped<ISastRepository, SastRepository>();
        Services.AddScoped<ICreateProjectCommandHandler, CreateProjectCommandHandler>();
        Services.AddScoped<IStartSastCommandHandler, StartSastCommandHandler>();
        Services.AddScoped<IStartDastCommandHandler, StartDastCommandHandler>();
        return Services;
    }
    private static string GetDatabasePath(string _path, string name = "appsec.sqlite")
    {
        var fullpath = Path.GetFullPath(_path);
        if (!Directory.Exists(fullpath))
        {
            Directory.CreateDirectory(fullpath);
        }
        return Path.GetFullPath(Path.Combine(fullpath, name));
    }
}
