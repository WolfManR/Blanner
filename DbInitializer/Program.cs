
using Blanner.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using Spectre.Console;

using System.Reflection;

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

Data.SingltonServices[Keys.LoggerFactory] = new LoggerFactory().AddSerilog(Log.Logger);

try {
	Log.Information("Starting application");

    while (true)
    {
		var menuItem = AnsiConsole.Prompt(Data.MenuPrompt);
		if(Data.Menu.TryGetValue(menuItem, out var action)) {
			try {
				action.Invoke();
			}
			catch (Exception ex) {
				Log.Error(ex, "Action ended with exception");
			}
		}
		if (Data.CloseApp) break;
    }
}
catch (Exception ex) {
	Log.Fatal(ex, "Application terminated unexpectedly");
}
finally {
	Log.CloseAndFlush();
}

static class Data {
	public static bool CloseApp { get; set; }

	public static Dictionary<string, object> Cache { get; } = [];
	public static Dictionary<string, object> SingltonServices { get; } = [];

	public static Dictionary<string, Func<object?>> Services { get; } = new() {
		[Keys.DbContext] = Factories.DbContext
	};


	public static Dictionary<string, Action> Menu { get; } = new() {
		["Setup connection string"] = Actions.SetupConnectionString,
		["Migrate database"] = Actions.Migrate,
		["[Red]Exit[/]"] = Actions.Exit,
	};

	public static SelectionPrompt<string> MenuPrompt { get; } = new SelectionPrompt<string>()
		.Title("Select action")
		.PageSize(12)
		.AddChoices(Menu.Keys);

	public static T Service<T>(string key, bool singleton = false) {
		if (singleton) {
            if (SingltonServices.TryGetValue(key, out var service))
            {
				return (T)service;
            }
			throw new InvalidOperationException("service not registered");
        }

		if(Services.TryGetValue(key, out var factory)) {
			return (T)factory.Invoke()!;
		}
		throw new InvalidOperationException("service not registered");
	} 
}

static class Keys {
	public const string ConnectionString = nameof(ConnectionString);
	public const string DbContext = nameof(DbContext);
	public const string LoggerFactory = nameof(LoggerFactory);
}

static class Actions {
	public static void SetupConnectionString() {
		var connectionString = AnsiConsole.Ask<string>("Enter connection string: ");
		Data.Cache[Keys.ConnectionString] = connectionString;
	}

	public static void Exit() => Data.CloseApp = true;

	public static void Migrate() {
		if (!Data.Services.TryGetValue(Keys.DbContext, out var service)) {
			return;
		}

		DbContext? context = (DbContext?)service.Invoke();
		if (context is null) return;


		if (context.Database.GetPendingMigrations().Count() == 0) {
			Log.Information("There no one pending migrations");
			return;
		}

		context.Database.Migrate();
	}
}

public static class Factories {
	public static ApplicationDbContext? DbContext() {
		if (!Data.Cache.TryGetValue(Keys.ConnectionString, out var cacheValue)) {
			Log.Warning("Connection string not registered");
			return null;
		}

		DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
		optionsBuilder
			.EnableDetailedErrors()
			.EnableSensitiveDataLogging()
			.UseLoggerFactory(Data.Service<ILoggerFactory>(Keys.LoggerFactory, singleton: true))
			.UseSqlServer((string) cacheValue, ctx => ctx.MigrationsAssembly(Assembly.GetAssembly(typeof(SQLServerMigrations.Mark))?.FullName));

		ApplicationDbContext dbContext = new(optionsBuilder.Options);
		return dbContext;
	}
}