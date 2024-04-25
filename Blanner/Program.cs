using Blanner.Api;
using Blanner.Components;
using Blanner.Components.Account;
using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Extensions;
using Blanner.Hubs;
using Blanner.Hubs.Clients;
using Blanner.Localizations;
using Blanner.Services;

using Coravel;
using Coravel.Events.Interfaces;
using Coravel.Scheduling.Schedule.Interfaces;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

using Serilog;

using System.Reflection;

Value<Contractor?, int>.SetKeySelector(x => x?.Id ?? 0);

Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateBootstrapLogger();

try {
	Log.Information("Starting web application");

	var builder = WebApplication.CreateBuilder(args);

	builder.Services.AddSerilog((services, logContext) => logContext
		.ReadFrom.Configuration(builder.Configuration)
		.ReadFrom.Services(services)
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.WriteTo.File(
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles", "Application", "diagnostics.txt"),
			rollingInterval: RollingInterval.Day,
			fileSizeLimitBytes: 10 * 1024 * 1024,
			retainedFileCountLimit: 2,
			rollOnFileSizeLimit: true,
			shared: true,
			flushToDiskInterval: TimeSpan.FromSeconds(1)));

	builder.Services.RegisterServices(builder.Configuration);

	var app = builder.Build();

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment()) {
		app.UseMigrationsEndPoint();
	}
	else {
		app.UseExceptionHandler("/Error", createScopeForErrors: true);
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	app.UseHttpsRedirection();

	app.UseStaticFiles();
	app.UseAntiforgery();

	app.UseSerilogRequestLogging();

	app.MapRazorComponents<App>()
		.AddInteractiveServerRenderMode();

	// Add additional endpoints required by the Identity /Account Razor components.
	app.MapAdditionalIdentityEndpoints();

	app.MapRoutes();

	app.MapHub<GoalsHub>("/hubs/goals");
	app.MapHub<JobsHub>("/hubs/jobs");
	app.MapHub<StickyHub>("/hubs/sticky");

	app.Services.SetupCoravel();

	app.Run();
}
catch(Exception ex) {
	Log.Fatal(ex, "Application terminated unexpectedly");
}
finally {
	Log.CloseAndFlush();
}



internal static class HostExtensions {

	public static void RegisterServices(this IServiceCollection services, ConfigurationManager configuration) {
		// Add services to the container.
		services.AddRazorComponents()
			.AddInteractiveServerComponents();

		services.AddCascadingAuthenticationState();
		services.AddScoped<IdentityUserAccessor>();
		services.AddScoped<IdentityRedirectManager>();
		services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

		services
			.AddAuthentication(options =>
			{
				options.DefaultScheme = IdentityConstants.ApplicationScheme;
				options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
			})
			.AddIdentityCookies();

		var dbProvider = configuration["DBProvider"] ?? throw new InvalidOperationException("Database provider not selected.");
		var connectionString = configuration.GetConnectionString(dbProvider) ?? throw new InvalidOperationException($"Connection string '{dbProvider}' not found.");
		services.AddDbContext<ApplicationDbContext>(options => {
			options.UseSqlServer(connectionString, ctx => ctx.MigrationsAssembly(Assembly.GetAssembly(typeof(SQLServerMigrations.Mark))?.FullName));
		});
		services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddSignInManager()
			.AddDefaultTokenProviders();

		services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

		// Coravel
		services
			.AddScheduler()
			.AddQueue()
			.AddEvents();

		services
			.AddBlazorContextMenu();

		services
			.AddScoped<StickyNoteClient>()
			.AddScoped<JobsClient>()
			.AddScoped<GoalsClient>();

		services.AddSignalR();
		services.AddHttpClient();

		services.AddFluentUIComponents();
		services.AddDataGridEntityFrameworkAdapter();



		services.AddScoped<ContractorsRepository>();
		services.AddScoped<GoalsRepository>();
		services.AddScoped<ActiveGoalsRepository>();
		services.AddScoped<JobsRepository>();
		services.AddScoped<UsersRepository>();

		services.AddSingleton<LocalizationManager>();
		services.AddSingleton<ILocalizationManager>(p => p.GetRequiredService<LocalizationManager>());
		services.AddSingleton<ILocalization>(p => p.GetRequiredService<LocalizationManager>());

		services.AddScoped<ClipboardService>();

		services.RegisterScheduledJobs();
		services.RegisterEvents();
	}

	public static IServiceProvider SetupCoravel(this IServiceProvider services) {
		services.UseScheduler(ConfigureScheduledJobs);
		
		services.ConfigureEvents(services.ConfigureEvents());

		return services;
	}

	public static void RegisterScheduledJobs(this IServiceCollection services) {
		
	}
	public static void ConfigureScheduledJobs(IScheduler scheduler) {
		
	}

	public static void RegisterEvents(this IServiceCollection services) {

	}
	public static IServiceProvider ConfigureEvents(this IServiceProvider services, IEventRegistration registration) {
		

		return services;
	}
}
