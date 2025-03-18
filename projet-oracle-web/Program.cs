using projet_oracle_web.Services;

namespace projet_oracle_web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			// Services to connect database Oracle
			builder.Services.AddSingleton<OracleDbContext>();
			builder.Services.AddScoped<EmployeeService>();
            builder.Services.AddScoped<DepartementService>();

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

            /*app.UseEndpoints(endpoints =>
            {
                // Routes définies ici, pas besoin de contrôleur dédié pour ChoiciData
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                // Exemple de routes pour ListerEmploye et ListerDepartement si ce n'est pas déjà défini
                endpoints.MapControllerRoute(
                    name: "ListerEmploye",
                    pattern: "ListerEmploye",
                    defaults: new { controller = "Employe", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "ListerDepartement",
                    pattern: "ListerDepartement",
                    defaults: new { controller = "Departement", action = "Index" });
            });*/

            app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}