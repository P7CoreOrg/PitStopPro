﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using GQL.GraphQLCore;
using GQL.GraphQLCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MultiAuthority.AccessTokenValidation;
using P7Core.ObjectContainers.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using GQL.Rollup.Extensions;
using Microsoft.Extensions.HealthChecks;

namespace GQL.GraphQLHost.Core
{
    public abstract class GraphQLRollupStartup<T> : Rollup.Extensions.AspNetCoreExtensions.IGraphQLRollupRegistrations
        where T : class
    {
        protected readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        protected ILogger<T> _logger;

        public GraphQLRollupStartup(IHostingEnvironment env, IConfiguration configuration, ILogger<T> logger)
        {
            _hostingEnvironment = env;
            Configuration = configuration;
            _logger = logger;
        }
        public abstract void AddGraphQLApis(IServiceCollection services);
        public abstract void AddGraphQLFieldAuthority(IServiceCollection services);
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddObjectContainer();  // use this vs a static to cache class data.
            services.AddOptions();
            services.AddDistributedMemoryCache();
            services.AddGraphQLPlayRollup(this);
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsBuilder => corsBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Daffy Duck",
                    policy => { policy.RequireClaim("client_namespace", "Daffy Duck"); });
            });
            var scheme = Configuration["authValidation:scheme"];

            var section = Configuration.GetSection("InMemoryOAuth2ConfigurationStore:oauth2");
            var oauth2Section = new Oauth2Section();
            section.Bind(oauth2Section);

            var query = from item in oauth2Section.Authorities
                        where item.Scheme == scheme
                        select item;
            var wellknownAuthority = query.FirstOrDefault();

            var authority = wellknownAuthority.Authority;
            List<SchemeRecord> schemeRecords = new List<SchemeRecord>()
            {  new SchemeRecord()
                {
                    Name = scheme,
                    JwtBearerOptions = options =>
                    {
                        options.Authority = authority;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {

                                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
                                if (identity != null)
                                {
                                    // Add the access_token as a claim, as we may actually need it
                                    var accessToken = context.SecurityToken as JwtSecurityToken;
                                    if (accessToken != null)
                                    {
                                        if (identity != null)
                                        {
                                            identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                        }
                                    }
                                }

                                return Task.CompletedTask;
                            }
                        };
                    }

                },
            };

            services.AddAuthentication("Bearer")
                .AddMultiAuthorityAuthentication(schemeRecords);


            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddDefaultHttpClientFactory();

            // Build the intermediate service provider then return it
            services.AddSwaggerGen(c =>
            {
                var assembly = typeof(T).Assembly;
                c.SwaggerDoc("v1", new Info { Title = $"{assembly.GetName().Name}", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                AddHealthChecks(checks);
            });
            AddAdditionalServices(services);
            return services.BuildServiceProvider();
        }

        protected abstract void AddAdditionalServices(IServiceCollection services);
        protected abstract void AddHealthChecks(HealthCheckBuilder checks);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            OnConfigureStart(app, env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQLPlayApiOnly V1");
            });
            OnConfigureEnd(app, env);
        }

        protected abstract void OnConfigureEnd(IApplicationBuilder app, IHostingEnvironment env);
        protected abstract void OnConfigureStart(IApplicationBuilder app, IHostingEnvironment env);
    }
}
