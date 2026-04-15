using ProyAdoPet.DAO;
using ProyAdoPet.Repository;
using ProyAdoPet.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMascota, MascotaDAO>();
builder.Services.AddScoped<MascotaService>();
builder.Services.AddScoped<IUsuario, UsuarioDAO>();
builder.Services.AddScoped<UsuarioService>();


//sesion
builder.Services.AddDistributedMemoryCache(); //guarda sesion en ram
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); //30 inactivo borra sesion
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Mascotas}/{id?}");

app.Run();
