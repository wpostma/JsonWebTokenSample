
// derived from https://github.com/mrsheepuk/ASPNETSelfCreatedTokenAuthExample
// ported to .net core 1.0 rtm

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using JsonWebTokenSample.Common;
using JsonWebTokenSample.Repository;


using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace JsonWebTokenSample
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnv; // this lets us detect that we'rd in development, etc, other places than in Startup()

        public static string[] args;
        
        private KeyArtifacts _keyArtifacts;

        public Startup(IHostingEnvironment env)
        {
            _keyArtifacts = new KeyArtifacts();
            _hostingEnv = env;

            // If env.ContentRootPath is not set properly (to a dir containing appsettings.json) this next block of code will not find our configuration and we want to raise an exception.

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", reloadOnChange: true, optional: true);

                // Note: hosting.json config builder is a different config builder!

                if (env.IsEnvironment("Development"))
                {
                    // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                    builder.AddApplicationInsightsSettings(developerMode: true);
                }
              

            builder.AddEnvironmentVariables();



            // After this point, configuration gets set.
          Configuration = builder.Build();


            // TODO: setup logging.
            
    

           string keyfilename = RSAKeyUtils.InitializeKeyArtifacts(env.WebRootPath, _keyArtifacts);
          //  Log.Warning($"Auth-Environment: {env.EnvironmentName} ");
          //  Log.Warning($"Auth-artifact: {_keyArtifacts}");
          //  Log.Warning($"Auth-secret:{keyfilename}");



        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Authorization Api: ConfigureServices is where we Add framework services.
            //services.AddApplicationInsightsTelemetry(Configuration);

#if DATABASE_SUPPORT
            // Add EF services to the services container
            string sqlConnection = "";
            if (Configuration != null)
            {
                sqlConnection = Configuration["Data:DefaultConnection:ConnectionString"];
            }
            bool useInMemoryStore = (Configuration == null)||(string.IsNullOrEmpty(sqlConnection));


            
            // using for unit test
            if (useInMemoryStore)
            { // In a lightweight integration test environment, or for some agile development purposes, we might want to run this service without a real database.
                services.AddEntityFramework()
                        .AddEntityFrameworkInMemoryDatabase()
                        .AddDbContext<Repository.DbContext>(options =>
                            options.UseInMemoryDatabase());
            }
            else // using in production and development
            {
                services.AddEntityFramework()
                        .AddEntityFrameworkSqlServer()
                        .AddDbContext<Repository.DbContext>(options =>
                            options.UseSqlServer(sqlConnection));
            }
            */

    
            //Configure the Repository instance in request scope
            services.AddScoped<IUserRepository, UserRepository>();

            //Configure the WorklistDbContext Create a new service instance each time accessed
            services.AddTransient<Repository.DbContext, Repository.DbContext>();
#endif


            services.AddSingleton<SigningCredentials>( _keyArtifacts.getSigningCredentials() );
            services.AddSingleton<JwtBearerOptions>( _keyArtifacts.getJwtBearerOptions() );

            // Configure Authentication
           
            
            services.Configure<AuthorizationOptions>(options =>
            {
                options.AddPolicy
                (
                    "JsonWebTokenSamplePolicy",
                    new AuthorizationPolicyBuilder().
                        AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).
                            RequireAuthenticatedUser().
                            Build()
                );
            }); 




            services.AddMvc();

            // SWASHBUCKLE+SWAGGER:
#if SWAGGER
            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Authorization API",
                    Description = "A simple api for authorization",
                    TermsOfService = "None"
                });

            });

            if (_hostingEnv.IsDevelopment())
            {
                services.ConfigureSwaggerGen(options =>
                {
                    options.DescribeAllEnumsAsStrings();
                    options.IncludeXmlComments( GetXmlCommentsPath());
                });

            };
#endif



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            //loggerFactory.AddSerilog();  // TODO: Add logging.


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                // app.UseExceptionHandler("/Home/Error");
                //Extension in Middle-ware:Add Exception Handler to catch all exception happening in the pipeline

                // app.UseMyExceptionHandler();  /// TODO: Make JsonWebTokenSample.Fhir.Api.Middlewares general and move to common.
            }

            if (Configuration != null) // disable telemetry during integration tests.
            {
                var enableTelemetry = Configuration["Telemetry"];
                if (enableTelemetry == "true")
                {
                    app.UseApplicationInsightsRequestTelemetry();
                    app.UseApplicationInsightsExceptionTelemetry();
                }
            }

            app.UseJwtBearerAuthentication(_keyArtifacts.getJwtBearerOptions());

            app.UseStaticFiles();

            app.UseMvc();


        }

        // This is replaced by the contents of Program.cs:
        // xxx: public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}

