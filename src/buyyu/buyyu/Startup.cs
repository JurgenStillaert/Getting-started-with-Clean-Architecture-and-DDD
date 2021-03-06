using buyyu.BL;
using buyyu.BL.Interfaces;
using buyyu.Data;
using buyyu.Data.Repositories;
using buyyu.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace buyyu
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
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "buyyu", Version = "v1" });
			});

			services.AddDbContext<BuyyuDbContext>(opt =>
			{
				opt.UseSqlServer(Configuration.GetConnectionString("BuyyuDbContext")).EnableSensitiveDataLogging(true);
			});

			//Repositories
			services.AddTransient<IProductRepository, ProductRepository>();
			services.AddTransient<IOrderRepository, OrderRepository>();
			services.AddTransient<IOrderStateRepository, OrderStateRepository>();
			services.AddTransient<IPaymentRepository, PaymentRepository>();
			services.AddTransient<IWarehouseRepository, WarehouseRepository>();

			//Services
			services.AddTransient<IProductService, ProductService>();
			services.AddTransient<IOrderService, OrderService>();
			services.AddTransient<IMailService, MailService>();
			services.AddTransient<IWarehouseService, WarehouseService>();
			services.AddTransient<IPaymentService, PaymentService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "buyyu v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}