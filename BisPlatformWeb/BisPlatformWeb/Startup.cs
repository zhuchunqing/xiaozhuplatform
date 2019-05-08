using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BisPlatform.Data.Common;
using BisPlatformWeb.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;
using System.Data;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Hangfire.MySql;
using Hangfire.MySql.Core;
using BisPlatformWeb.Hangfire.Interface;
using BisPlatformWeb.Hangfire.Implementation;
using BisPlatformWeb.Common.Redis;
using Microsoft.Extensions.Caching.Redis;

namespace BisPlatformWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "bisplat",
                        ValidAudience = "bisplat",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecurityKey"]))
                    };
                });
            #endregion
            services.AddMvc()
                .AddJsonOptions(options => {
                    //配置大小写问题，默认是首字母小写
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #region MySql
            services.AddDbContext<UsersContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MyOracleUser"));
            });
            #endregion
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                });
                c.SwaggerDoc("v2", new Info
                {
                    Version = "v2",
                });
                var basePath = AppContext.BaseDirectory;
                var assemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
                var fileName = System.IO.Path.GetFileName(assemblyName + ".xml");
                c.IncludeXmlComments(System.IO.Path.Combine(basePath, fileName));
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
            });
            #endregion
            #region cache
            services.AddMemoryCache();
            #endregion
            #region Handfire定时服务
            services.AddHangfire(x => x.UseStorage(new MySqlStorage(Configuration.GetConnectionString("MyOracleUser"), new MySqlStorageOptions() { TablePrefix = "Custom" })));
            services.AddTransient<IMyjob, Myjob>();
            #endregion
            #region redis
            //var tt = Configuration.GetSection("Redis:ConnectionString").Value;
            //var t= Configuration.GetSection("Redis:InstanceName").Value;
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Redis:ConnectionString").Value;
                options.InstanceName = Configuration.GetSection("Redis:InstanceName").Value;
                //options.InstanceName = "RedisNetCoreSample";
                //options.Configuration = "10.2.1.238:6379,password=Ybsx#123"; //Your Redis Connection

            });
            services.AddScoped<IRedisService, RedisService>();
            #endregion
            #region 跨域
            // 跨域配置
            services.AddCors(options =>
            {
                //BisPlatform是自定义的，可以自己改
                options.AddPolicy("BisPlatform", policy =>
                { 
                    policy.WithOrigins(Configuration.GetSection("Cors:Origins").Get<string[]>())
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                });
            });
            #endregion
            #region Autofac            
            return RegisterAutofac(services);//注册Autofac
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UsersContext context, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            #region jwt
            app.UseAuthentication();
            #endregion
            app.UseHttpsRedirection();
            app.UseMvc();
            InItUserDatabase(app, context);
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "朱春清");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "陈展");
            });
            #endregion
            #region Nlog
            //ILoggerFactory loggerFactory
            loggerFactory.AddNLog();
            NLog.LogManager.LoadConfiguration("NLog.config"); //填入创建的文件的名称
            #endregion
            #region 全局异常
            app.UseErrorHandling();
            #endregion
            #region hangfire定时服务
            //只允许本地服务器访问
            var options = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            };
            app.UseHangfireDashboard("/hangfire", options);
            //配置任务属性
            var jobOptions = new BackgroundJobServerOptions
            {
                WorkerCount = Environment.ProcessorCount * 5, //并发任务数
                ServerName = "hangfire1",//服务器名称
            };
            app.UseHangfireServer(jobOptions);
            RecurringJob.AddOrUpdate(() => Console.WriteLine("xiaoma"), Cron.HourInterval(1));
            RecurringJob.AddOrUpdate<IMyjob>(a => a.RunAtTimeOf(DateTime.Now), Cron.Minutely());
            #endregion
        }
        /// <summary>
        /// 创造数据库
        /// </summary>
        /// <param name="app"></param>
        /// <param name="context"></param>
        public void InItUserDatabase(IApplicationBuilder app, UsersContext context)
        {
            context.Database.EnsureCreated();
        }
        #region autofac
        private IServiceProvider RegisterAutofac(IServiceCollection services)
        {
            //实例化Autofac容器
            var builder = new ContainerBuilder();
            //将Services中的服务填充到Autofac中
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var Container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(Container);
        }
        public class AutofacModuleRegister : Autofac.Module
        {
            //重写Autofac管道Load方法，在这里注册注入
            protected override void Load(ContainerBuilder builder)
            {
                //注册Service中的对象,Service中的类要以Service结尾，否则注册失败
                builder.RegisterAssemblyTypes(GetAssemblyByName("BisInterface")).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces();
                //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
                builder.RegisterAssemblyTypes(GetAssemblyByName("BisImplementation")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
                //data layer
                builder.Register(context => new UsersContext(context.Resolve<DbContextOptions<UsersContext>>()))
                    .As<IUserContext>().InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
                //services.AddScoped<IRedisService, RedisService>();
            }
            /// <summary>
            /// 根据程序集名称获取程序集
            /// </summary>
            /// <param name="AssemblyName">程序集名称</param>
            /// <returns></returns>
            public static Assembly GetAssemblyByName(String AssemblyName)
            {
                return Assembly.Load(AssemblyName);
            }
        }
        #endregion
        #region hangfire
        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            //这里需要配置权限规则
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
        #endregion
    }
}
