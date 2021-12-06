using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using event_client_app.Configure;
using Microsoft.AspNetCore.Mvc;

namespace event_client_app
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DBAPPContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"),
                MySqlServerVersion.LatestSupportedServerVersion));
            services.AddControllers();
            // services.AddAuthentication();
            services.AddMvc(opt => { opt.UseCentralRouterPrefix(new RouteAttribute("api/v1")); });
            services.AddCors(option => option.AddPolicy("AllowCors", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod()//.WithMethods("Origin, X-Requested-With, Content-Type, Accept, X-Authorization")
                    .AllowAnyHeader();
                //.WithOrigins("http://localhost:8080")
            }));
            // services.AddCors(option =>
            // {
            //     option.AddPolicy(name: "Access-Control-Allow-Origin",
            //         builder => builder.WithOrigins("*"));
            //     option.AddPolicy("Access-Control-Allow-Headers",
            //         builder => builder.WithHeaders("Origin, X-Requested-With, Content-Type, Accept, X-Authorization"));
            //     // option.AddPolicy("Access-Control-Allow-Methods",
            //     //     builder => builder.WithMethods("GET, POST, PATCH, PUT, DELETE"));
            //     option.AddPolicy("Access-Control-Allow-Methods",
            //         builder => builder.AllowAnyMethod());
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowCors");
            // app.UseCors("Access-Control-Allow-Origin");
            // app.UseCors("Access-Control-Allow-Headers");
            // app.UseCors("Access-Control-Allow-Methods");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            // 

            // app.UseAuthorization();
            // app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/", async context =>
                // {
                //     
                //     
                //     await context.Response.WriteAsync("Hello World!");
                // });
                endpoints.MapControllers();
            });

            //MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}