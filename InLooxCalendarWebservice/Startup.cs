using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace InLooxCalendarWebservice
{
    public class Startup
    {
        private const string SERVICE_URL = "https://app.inlooxnow.com/"; // <- insert your IWA service url here in case you run InLoox on premise
        private const string MY_TASKS_ROUTE = "my-tasks"; // <- change this if you like
        private const string ACCESS_TOKEN_PARAMETER_NAME = "access_token";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // standard route
                if (env.IsDevelopment())
                {
                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Running");
                    });
                }

                // my tasks route
                endpoints.MapGet("/" + MY_TASKS_ROUTE, async context =>
                {
                    await ProcessMyTasksRequest(context);
                });
            });
        }

        private static async Task<string> GetToken(HttpContext context)
        {
            // access token has to be a get parameter
            // Outlook and others are unable to post parameters/headers
            var token = context.Request.Query[ACCESS_TOKEN_PARAMETER_NAME];
            if (token.Count == 0)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }

            return token;
        }

        private static async Task ProcessMyTasksRequest(HttpContext context)
        {
            // use the token for authorization
            var token = await GetToken(context);

            // connect to service
            var service = new IwaService();
            if (await service.Connect(new System.Uri(SERVICE_URL), token))
            {
                // read user's tasks
                var tasks = await service.GetMyTasks();

                // create ICS feed and return
                var feed = new IcsFeed(tasks);
                await context.Response.WriteAsync(feed.Serialize());
            }
        }
    }
}
