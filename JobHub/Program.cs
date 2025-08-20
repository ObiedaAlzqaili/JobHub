
using JobHub.Data;
using JobHub.Interfaces.AiInterfaces;
using JobHub.Interfaces.RepositoriesInterfaces;
using JobHub.Interfaces.ServicesInterfaces;
using JobHub.Models;
using JobHub.repositories;
using JobHub.Services;
using JobHub.Services.FileHandler;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using NuGet.Packaging;



namespace JobHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddSingleton<Kernel>(sp => {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion(
                    modelId: "gpt-5", // or your model name
                    apiKey: builder.Configuration["OpenAi:Key"]
                );
                return kernelBuilder.Build();
            });


            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IProfileReposotity, ProfileRepository>();
            builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();
            builder.Services.AddScoped<IOpenAiKeywordExtraction, OpenAiKeywordExtraction>();
            builder.Services.AddScoped<IJobMatchingService, JobMatchingService>();
            builder.Services.AddScoped<IResumeProcessingService, ResumeProcessingService>();








            builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.Configure<EmailSettings>(
              builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<EmailSettings>>().Value);
            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            //end for the mailkit

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.Use(async (context, next) =>
            {
                if (context.User?.Identity?.IsAuthenticated == true && context.Request.Path == "/")
                {
                    var userManager = context.RequestServices.GetRequiredService<UserManager<Person>>();
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null && await userManager.IsInRoleAsync(user, "Company"))
                    {
                        context.Response.Redirect("/Company/Profile");
                        return;
                    }

                    context.Response.Redirect("/Home/Index");
                    return;
                }

                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}