using Blanner.Api;
using Blanner.Client.Pages;
using Blanner.Components;
using Blanner.Components.Account;
using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Extensions;
using Blanner.Hubs;
using Blanner.Services;
using Blanner.Services.Jobs;
using Coravel;
using Coravel.Events.Interfaces;
using Coravel.Scheduling.Schedule.Interfaces;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Value<Contractor?, int>.SetKeySelector(x => x?.Id ?? 0);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents()
	.AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
	{
		options.DefaultScheme = IdentityConstants.ApplicationScheme;
		options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
	})
	.AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
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

builder.Services.AddSignalR();
builder.Services.AddHttpClient();

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
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
	.AddInteractiveServerRenderMode()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(Auth).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapRoutes();

app.MapHub<GoalsHub>("/hubs/goals");

app.Services.SetupCoravel();

app.Run();


internal static class HostExtensions {

	public static void RegisterServices(this IServiceCollection services) {
		services.AddScoped<ContractorsRepository>();
		services.AddScoped<GoalsRepository>();
		services.AddScoped<ActiveGoalsRepository>();
		services.AddScoped<JobsRepository>();

		services.RegisterScheduledJobs();
		services.RegisterEvents();
	}

	public static IServiceProvider SetupCoravel(this IServiceProvider services) {
		services.UseScheduler(ConfigureScheduledJobs);
		
		services.ConfigureEvents(services.ConfigureEvents());

		return services;
	}

	public static void RegisterScheduledJobs(this IServiceCollection services) {
		services.AddTransient<TimeNotifier>();
	}
	public static void ConfigureScheduledJobs(IScheduler scheduler) {
		scheduler.Schedule<TimeNotifier>().EverySecond().PreventOverlapping(nameof(TimeNotifier));
	}

	public static void RegisterEvents(this IServiceCollection services) {

	}
	public static IServiceProvider ConfigureEvents(this IServiceProvider services, IEventRegistration registration) {
		

		return services;
	}
}
