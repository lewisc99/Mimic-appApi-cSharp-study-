using _1_mimicApi_study_test.Data;
using _1_mimicApi_study_test.Helpers;
using _1_mimicApi_study_test.V1.Repositories;
using _1_mimicApi_study_test.V1.Repositories.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace _1_mimicApi_study_test
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

            services.AddMvc();


            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DTOMapperProfile());
            });


            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);



            services.AddDbContext<MimicContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPalavraRepository, PalavraRepository>();



            services.AddMvc(options => options.EnableEndpointRouting = false);


            services.AddApiVersioning(
                cfg =>
                {

                    cfg.ApiVersionReader = new HeaderApiVersionReader(); //help to versioning api which method each API will show in the swagger


                  // cfg.ReportApiVersions = true;
                    cfg.AssumeDefaultVersionWhenUnspecified = true;
                   
                    cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

                });


            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.DefaultApiVersionParameterDescription = "Do NOT modify api-version!";
            });






            services.AddSwaggerGen(c =>
            {
             
                c.SwaggerDoc("v2.0", new OpenApiInfo
                {
                    Version = "v2.0",
                    Title = "MIMIC - API V2"
                });

                c.SwaggerDoc("v1.1", new OpenApiInfo
                {
                    Version = "v1.1",
                    Title = "MImic - API V1.1"
                });

                c.SwaggerDoc("v1.0", new OpenApiInfo
                { Title = "MImic - API V1.0", Version = "v1.0" });

                var caminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;
                var NomeProjeto = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var caminhoArquivoXMlComentario = Path.Combine(caminhoProjeto, NomeProjeto);


                c.IncludeXmlComments(caminhoArquivoXMlComentario);


                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiVersionAttribute>().SelectMany(attr => attr.Versions);
                    var maps = methodInfo.GetCustomAttributes(true).OfType<MapToApiVersionAttribute>().SelectMany(attr => attr.Versions).ToArray();
                    version = version.Replace("v", "");
                    return versions.Any(v => v.ToString() == version && maps.Any(v => v.ToString() == version));
                });


                c.ResolveConflictingActions(apiDescription => apiDescription.First());


            });


         
            



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


              
            }
            app.UseStatusCodePages();
              app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc();

            app.UseSwagger();

            app.Use((context, next) =>
            {
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                return next.Invoke();
            });

            app.UseSwaggerUI(cfg =>
            {
                cfg.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MimicAPI 1.0");
                cfg.SwaggerEndpoint("/swagger/v1.1/swagger.json", "MimicAPI 1.1");
                cfg.SwaggerEndpoint("/swagger/v2.0/swagger.json", "MimicAPI 2.0");


                cfg.RoutePrefix = string.Empty;
            });



            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});


        }
    }
}
