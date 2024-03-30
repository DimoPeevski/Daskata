var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationIdentity(builder.Configuration);

builder.Services.AddApplicationDbContext(builder.Configuration);

builder.Services.AddApplicationServices();

await builder.Services.RolesSeedAsync(builder.Configuration);

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/User/Login";
    config.LogoutPath = "/User/Logout";
    config.AccessDeniedPath = "/User/AccessDenied";
});

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapRazorPages();

await app.RunAsync();

