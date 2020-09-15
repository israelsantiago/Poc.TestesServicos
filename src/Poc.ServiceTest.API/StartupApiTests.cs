using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoC.TestesServicos.API.Configs;
using PoC.TestesServicos.Core.Interfaces;
using PoC.TestesServicos.Core.Services;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Data.Couchbase.Providers;
using PoC.TestesServicos.Data.Couchbase.Repositories;

namespace PoC.TestesServicos.API
{
    public class StartupApiTests
    {
        public StartupApiTests(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CepApiOptions>(Configuration.GetSection(nameof(CepApiOptions)));
            
            services.AddSingleton<IContextConfiguration, DataContextConfiguration>();
   
            services.AddDbContext<UsersDataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(Configuration.GetSection("ConnectionStrings:UsersDb").Value),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 20, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                    });
            });            
    
            services.AddSingleton<ICouchbaseProvider, CouchbaseProvider>();
            services.AddSingleton<IDocumentsRepository, DocumentsRepository>();
            
            services.AddTransient<ICustomerService, CustomerService>(); 
            services.AddTransient<ICepService, CepService>();            

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }

    }
}