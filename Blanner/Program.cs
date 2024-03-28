using Blanner.Api;
using Blanner.Components;
using Blanner.Components.Account;
using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Extensions;
using Blanner.Hubs;
using Blanner.Hubs.Clients;
using Blanner.Localizations;

using Coravel;
using Coravel.Events.Interfaces;
using Coravel.Scheduling.Schedule.Interfaces;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

Value<Contractor?, int>.SetKeySelector(x => x?.Id ?? 0);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddIdentityCookies();

var dbProvider = builder.Configuration["DBProvider"] ?? throw new InvalidOperationException("Database provider not selected.");
var connectionString = builder.Configuration.GetConnectionString(dbProvider) ?? throw new InvalidOperationException($"Connection string '{dbProvider}' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => {
	if (dbProvider.Equals("sqlite", StringComparison.CurrentCultureIgnoreCase)) options.UseSqlite(connectionString, ctx => ctx.MigrationsAssembly(Assembly.GetAssembly(typeof(SQLiteMigrations.Mark)).FullName));
	else options.UseSqlServer(connectionString, ctx => ctx.MigrationsAssembly(Assembly.GetAssembly(typeof(SQLServerMigrations.Mark)).FullName)); 
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddSignInManager()
	.AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<User>, IdentityNoOpEmailSender>();

// Coravel
builder.Services
	.AddScheduler()
	.AddQueue()
	.AddEvents();

builder.Services
	.AddBlazorContextMenu();

builder.Services.AddScoped<StickyNoteClient>();

builder.Services.AddSignalR();
builder.Services.AddHttpClient();

builder.Services.AddFluentUIComponents();
builder.Services.AddDataGridEntityFrameworkAdapter();

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapRoutes();

app.MapHub<GoalsHub>("/hubs/goals");
app.MapHub<StickyHub>("/hubs/sticky");

app.Services.SetupCoravel();

app.Run();


internal static class HostExtensions {

	public static void RegisterServices(this IServiceCollection services) {
		services.AddScoped<ContractorsRepository>();
		services.AddScoped<GoalsRepository>();
		services.AddScoped<ActiveGoalsRepository>();
		services.AddScoped<JobsRepository>();
		services.AddScoped<UsersRepository>();

		services.AddSingleton<LocalizationManager>();
		services.AddSingleton<ILocalizationManager>(p => p.GetRequiredService<LocalizationManager>());
		services.AddSingleton<ILocalization>(p => p.GetRequiredService<LocalizationManager>());

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
