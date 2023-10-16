using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PollaEngendrilClientHosted.Server.Services;
using PollaEngendrilClientHosted.Server.Services.ScoringStaregies;
using Microsoft.EntityFrameworkCore;
using PollaEngendrilClientHosted.Server.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PollaEngendrilClientHosted.Server.Services.UserEligibleSpecification;

namespace PollaEngendrilClientHosted.Server;
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = builder.Configuration["Auth0:Audience"],
                    ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}",
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<IPredictionService, PredictionService>();
        builder.Services.AddScoped<IFixturesService, FixturesService>();
        builder.Services.AddScoped<IUsersService, UsersService>();
        builder.Services.AddScoped<IPredictionStrategy, ExactScorePredictionStrategy>();
        builder.Services.AddScoped<IPredictionStrategy, WinnerOrTiePredictionStrategy>();
        builder.Services.AddScoped<IUserEligibleSpecification, UserEligibleByEmailPatternSpecification>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        app.Run();
    }
}