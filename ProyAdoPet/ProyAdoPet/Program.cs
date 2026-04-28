using ProyAdoPet.DAO;
using ProyAdoPet.Repository;
using ProyAdoPet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Dependency Injection
builder.Services.AddScoped<IMascota, MascotaDAO>();
builder.Services.AddScoped<MascotaService>();

builder.Services.AddScoped<IUsuario, UsuarioDAO>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ISolicitudAdopcion, SolicitudAdopcionDAO>();
builder.Services.AddScoped<SolicitudService>();

// SESSION
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// COOKIE AUTH
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Login/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// 🔥 RUTA CORRECTA (ESTO ARREGLA EL ERROR)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Mascotas}/{id?}");

app.Run();
