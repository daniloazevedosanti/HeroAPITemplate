﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dto;
using AutoMapper;
using Domain.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Repository.Connection;
using Repository.Contracts;
using Repository.Reposiroies;
using Service.Contracts;
using Service.Services;

namespace Api
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
            services.AddSingleton<IHeroService, HeroService>();
            services.AddSingleton<IHeroRepository, HeroRepository>();
            services.AddSingleton<IConnectionFactory, DeafultSqlConnectionFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            AutoMapperConfig(services);
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AutoMapperConfig(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HeroDto, Hero>();
                cfg.CreateMap<Hero, HeroDto>();
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
