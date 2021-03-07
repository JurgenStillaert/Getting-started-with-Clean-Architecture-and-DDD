using buyyu.BL;
using buyyu.BL.Interfaces;
using buyyu.Data;
using buyyu.Data.Repositories;
using buyyu.Data.Repositories.Interfaces;
using buyyu.DDD;
using buyyu.Domain.Order;
using buyyu.Domain.Payment;
using buyyu.Domain.Shared;
using buyyu.Domain.Shipment;
using buyyu.Domain.Warehouse;
using buyyu.web.Infrastructure;
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
			services.AddMediatrOnUseCases();

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
			services.AddTransient<IWarehouseRepository, WarehouseRepository>();

			services.AddTransient<IRepository<OrderRoot, OrderId>, Data.Repositories.Commands.OrderRepository>();
			services.AddTransient<IRepository<PaymentRoot, PaymentId>, Data.Repositories.Commands.PaymentRepository>();
			services.AddTransient<IRepository<ShipmentRoot, OrderId>, Data.Repositories.Commands.ShipmentRepository>();
			services.AddTransient<IRepository<WarehouseRoot, ProductId>, Data.Repositories.Commands.WarehouseRepository>();

			//Services
			services.AddTransient<IMailService, MailService>();
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