using FluentValidation;
using FluentValidation.AspNetCore;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators;
using Insolvency.CalculationsEngine.Redundancy.API.Infrastructure.Middlewares.Validators.IrregularHourWorkerHPA;
using Insolvency.CalculationsEngine.Redundancy.Common.ConfigLookups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;

namespace Insolvency.CalculationsEngine.Redundancy.API
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
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RPS Calculations API", Version = "V1"});
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddOptions();
            services.Configure<ConfigLookupRoot>(Configuration);

            services.AddHealthChecks();

            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<APPACalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ApportionmentCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ArrearsOfPayCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<BasicAwardCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<CompensatoryNoticePayCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<HolidayCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<HolidayPayAccruedCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<HolidayTakenNotPaidCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<NoticePayCompositeCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<NoticeWorkedNotPaidCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ProjectedNoticeDateCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<ProtectiveAwardCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RedundancyPaymentCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<RefundOfNotionalTaxCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<IrregularHolidayCalculationRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<IrregularHolidayPayAccruedCalculationRequestValidator>();

            //Configure BL services
            ServicesInstaller.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPS Calculations API V1"); });
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

        }
    }
}