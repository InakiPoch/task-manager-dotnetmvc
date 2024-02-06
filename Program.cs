using tl2_tp10_2023_InakiPoch.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor(); //Allows HttpContextAccesor dependency inyection over RoleCheck

var connectionPath = builder.Configuration.GetConnectionString("boardConnection")!;
builder.Services.AddSingleton<string>(connectionPath);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITasksRepository, TasksRepository>();

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromSeconds(300);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<RoleCheck>();

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

//To access session's role on a Razor View
app.Use((context, next) =>
{
    context.Items["RoleCheck"] = context.RequestServices.GetRequiredService<RoleCheck>();
    return next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
