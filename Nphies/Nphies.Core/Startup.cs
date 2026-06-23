using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Nphies.Core.Brokers.Loggings;
using Nphies.Core.Data.Entities;
using Nphies.Core.Services.Claims;
using Nphies.Core.Services.Communication;
using Nphies.Core.Services.Logger;
using Nphies.Core.Services.PDFAttachment;
using Nphies.Core.Services.Schedule;

namespace Nphies.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
             Configuration = configuration;

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4201", "http://localhost:4200")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            services.AddControllers();
            services.AddHttpClient(); 
            AddServices(services);

            services.AddDbContext<ZyklusCoreContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddSwaggerGen(options =>
            {
                var openApiInfo = new OpenApiInfo
                {
                    Title = "Nphies.Core",
                    Version = "v1"
                };

                options.SwaggerDoc(
                    name: "v1",
                    info: openApiInfo
                    );
            });
            services.AddMemoryCache();
        }
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<ILogService, LogService<BsonDocument>>();
            services.AddScoped<ISchedularService, SchedularService>();
            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<IPDFAttachmentService, PDFAttachmentService>();
            services.AddScoped<IClaimUpdate, ClaimUpdate>();
            services.AddSingleton<ILoggingBroker, LoggingBroker>();
            services.AddScoped<Nphies.Core.Services.Submission.ISubmissionService, Nphies.Core.Services.Submission.SubmissionService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nphies.Core v1");
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAngularApp");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
