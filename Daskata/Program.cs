var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationIdentity(builder.Configuration);
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/User/Login";
    config.LogoutPath = "/User/Logout";
    config.AccessDeniedPath = "/Error/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddRazorPages();

var app = builder.Build();

await builder.Services.RolesSeedAsync(builder.Configuration);
await ServiceCollectionExtension.InitializeApplicationData(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Error/404");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapDefaultControllerRoute();
app.MapRazorPages();

await app.RunAsync();
