using ProyAdoPet.DAO;
using ProyAdoPet.Repository;
using ProyAdoPet.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMascota, MascotaDAO>();
builder.Services.AddScoped<MascotaService>();
builder.Services.AddScoped<IUsuario, UsuarioDAO>();
builder.Services.AddScoped<UsuarioService>();


//sesion
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; //si no esta logueado
        options.AccessDeniedPath = "/Login/AccesoDenegado"; //donde se va sin permiso
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); 
    }); 

builder.Services.AddScoped<ISolicitud, SolicitudDAO>();

var app = builder.Build();

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

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Mascotas}/{id?}");

app.Run();
