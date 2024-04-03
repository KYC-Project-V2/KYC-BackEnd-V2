using KYCServiceApi.Controllers;
using KYCServiceApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository;
using Service;
using System.Text;

namespace KYCServiceApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration);
            services.AddSingleton<ISqlConnectionInformation>(new SqlConnectionInformation(Settings.appSettings));

            services.AddTransient<IService<PersonalInfo>, PersonalInfoService>();
            services.AddTransient<IRepository<PersonalInfo>, PersonalInfoRepository>();

            services.AddTransient<IService<BusinessInfo>, BusinessInfoService>();
            services.AddTransient<IRepository<BusinessInfo>, BusinessInfoRepository>();

            services.AddTransient<IService<State>, StateService>();
            services.AddTransient<IRepository<State>, StateRepository>();

            services.AddTransient<IService<Country>, CountryService>();
            services.AddTransient<IRepository<Country>, CountryRepository>();

            services.AddTransient<IService<AadharInfo>, AadharInfoService>();
            services.AddTransient<IRepository<AadharInfo>, AadharInfoRepository>();

            services.AddTransient<IService<VoterInfo>, VoterInfoService>();
            services.AddTransient<IRepository<VoterInfo>, VoterInfoRepository>();

            services.AddTransient<IService<PanCardInfo>, PanCardInfoService>();
            services.AddTransient<IRepository<PanCardInfo>, PanCardInfoRepository>();

            services.AddTransient<IService<DriverLicenseInfo>, DriverLicenseInfoService>();
            services.AddTransient<IRepository<DriverLicenseInfo>, DriverLicenseInfoRepository>();

            services.AddTransient<IService<BusinessPanCardInfo>, BusinessPanCardInfoService>();
            services.AddTransient<IRepository<BusinessPanCardInfo>, BusinessPanCardInfoRepository>();

            services.AddTransient<IService<BusinessInCorpInfo>, BusinessInCorpInfoService>();
            services.AddTransient<IRepository<BusinessInCorpInfo>, BusinessInCorpInfoRepository>();

            services.AddTransient<IService<OTPVerification>, OTPVerificationService>();
            services.AddTransient<IRepository<OTPVerification>, OTPVerificationRepository>();

            services.AddTransient<IService<DocumentVerification>, DocumentVerificationService>();
            services.AddTransient<IRepository<DocumentVerification>, DocumentVerificationRepository>();

            services.AddTransient<IService<RequestOrigin>, RequestOriginService>();
            services.AddTransient<IRepository<RequestOrigin>, RequestOriginRepository>();

            services.AddTransient<IService<Domain>, DomainService>();
            services.AddTransient<IRepository<Domain>, DomainRepository>();

            services.AddTransient<IService<EmailConfiguration>, EmailConfigurationService>();
            services.AddTransient<IRepository<EmailConfiguration>, EmailConfigurationRepository>();

            services.AddTransient<IService<Certificate>, CertificateService>();
            services.AddTransient<IRepository<Certificate>, CertificateRepository>();

            services.AddTransient<IService<APIConfiguration>, APIConfigurationService>();
            services.AddTransient<IRepository<APIConfiguration>, APIConfigurationRepository>();

            services.AddTransient<IService<TemplateConfiguration>, TemplateConfigurationService>();
            services.AddTransient<IRepository<TemplateConfiguration>, TemplateConfigurationRepository>();

            services.AddTransient<IService<TemplateType>, TemplateTypeService>();
            services.AddTransient<IRepository<TemplateType>, TemplateTypeRepository>();

            services.AddTransient<IService<CertificateConfirmation>, CertificateConfirmationService>();
            services.AddTransient<IRepository<CertificateConfirmation>, CertificateConfirmationRepository>();

            services.AddTransient<IService<RequestOrigin>, RequestOriginService>();
            services.AddTransient<IRepository<RequestOrigin>, RequestOriginRepository>();

            services.AddTransient<IService<PendingRequest>, PendingRequestService>();
            services.AddTransient<IRepository<PendingRequest>, PendingRequestRepository>();

            services.AddTransient<IService<CCAvenue>, CCAvenueService>();
            services.AddTransient<IService<Email>, EmailService>();

            services.AddTransient<IService<Payment>, PaymentService>();
            services.AddTransient<IRepository<Payment>, PaymentRepository>();

            services.AddTransient<IService<CCAvenueResponse>, CCAvenueResponseService>();
            services.AddTransient<IRepository<CCAvenueResponse>, CCAvenueRepository>();

            services.AddTransient<IService<PaymentConfiguration>, PaymentConfigurationService>();
            services.AddTransient<IRepository<PaymentConfiguration>, PaymentConfigurationRepository>();

            services.AddTransient<IService<Order>, OrderService>();

            services.AddTransient<IService<RequestVerification>, RequestVerificationService>();

            services.AddTransient<IService<Authentication>, AuthenticationService>();
            services.AddTransient<IRepository<Authentication>, AuthenticationRepository>();

            services.AddTransient<IJWTService, JWTService>();
            services.AddTransient<IJWTRepository, JWTRepository>();

            services.AddTransient<IService<SProvider>, ServiceProviderService>();
            services.AddTransient<IRepository<SProvider>, ServiceProviderRepository>();

            services.AddHttpClient();
            services.AddHealthChecks();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            services.AddControllersWithViews();
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = long.MaxValue;
            });
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
            });

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<Authorization>();
            //});

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseCors(x => x.AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                  .AllowCredentials());
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });


            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Document")),
                RequestPath = new PathString("/Document")
            });
        }

    }
}
