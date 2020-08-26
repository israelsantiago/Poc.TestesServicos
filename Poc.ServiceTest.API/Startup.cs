﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoC.TestesServicos.API.Configs;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Data.Couchbase.Providers;
using PoC.TestesServicos.Data.Couchbase.Repositories;

namespace PoC.TestesServicos.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UsersDataContext>();
            services.AddSingleton<IContextConfiguration, DataContextConfiguration>();

            services.AddSingleton<ICouchbaseProvider, CouchbaseProvider>();
            services.AddSingleton<IDocumentsRepository, DocumentsRepository>();

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