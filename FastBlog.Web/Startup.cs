using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using FastBlog.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using MySql.Data.MySqlClient;

namespace FastBlog.Web
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
            services.AddMvc();

            // 后台登录授权验证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/admin/account/login";
                options.LogoutPath = "/admin/account/logout";
                options.Cookie = new CookieBuilder { HttpOnly = true };
            });


            // 解决中文乱码
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            // 依赖注入
            DependecyRegistrar(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Article}/{action=List}/{id?}");

                // Areas 后台配置
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                  );
            });
        }

        void DependecyRegistrar(IServiceCollection services)
        {
            // IDbConnection 注入
            services.AddScoped<IDbConnection, MySqlConnection>(serviceProvider =>
            {
                var connection = new MySqlConnection(Configuration.GetConnectionString("DefaultConnection"));
                if (connection.State != ConnectionState.Closed) connection.Open();
                return connection;
            });

            #region - Services 注入
            Assembly[] assemblies = DependencyContext.Default.CompileLibraries
                .Where(lib => !lib.Serviceable && lib.Name.StartsWith("FastBlog", StringComparison.OrdinalIgnoreCase))
                .Select(p => { return AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(p.Name)); }).ToArray();
            var types = assemblies
                            .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IDependency))))
                            .ToArray();
            var interfaces = types.Where(t => t.IsInterface).ToList();
            var impements = types.Where(t => !t.IsAbstract).ToList();
            // var asd=  types.Select(c => c.GetInterfaces()).ToList();
            var didatas = interfaces
                .Select(t =>
                {
                    //if (t.IsGenericTypeDefinition)
                    //{
                    //    return new
                    //    {
                    //        serviceType = t,
                    //        implementationType = impements.FirstOrDefault(c => c.IsGenericTypeDefinition
                    //        && c.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == t.GetGenericTypeDefinition()))
                    //    };
                    //}
                    //else
                    return new
                    {
                        serviceType = t,
                        implementationType = impements.FirstOrDefault(c => t.IsAssignableFrom(c))
                    };
                }
                ).ToList();

            didatas.ForEach(t =>
            {
                if (t.implementationType != null)
                    services.AddScoped(t.serviceType, t.implementationType);
            });
            #endregion
        }
    }
}
