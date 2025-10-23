using DataVault.Repositories;
using DataVault.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
{
    var connProvider = sp.GetRequiredService<IConnectionStringProvider>();
    options.UseMySql(
        connProvider.GetConnectionString(),
        ServerVersion.AutoDetect(connProvider.GetConnectionString()),
        mySqlOptions => mySqlOptions.EnableStringComparisonTranslations() // 👈 ADICIONADO
    );
});

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IFilesRepository, FilesRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddHttpClient<DataVault.Services.IAService>();
builder.Services.AddScoped<DataVault.Services.IAService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseSession();
app.Run();
