using API.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IUserRepositories, UserRepositories>();
builder.Services.AddSingleton<ICityRepositories, CityRepositories>();
builder.Services.AddScoped<CommonRepositories>();


builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// builder.Services.AddSingleton<IUserRepositories, UserRepositories>();
// builder.Services.AddSingleton<ICityRepositories, CityRepositories>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
    .AddCookie("MyCookieAuthenticationScheme", option =>
    {
        option.LogoutPath = "/User/Login";
        option.AccessDeniedPath = "/User/AccessDenied";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Set your desired expiration time
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5); // Set your desired timeout for session inactivity
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Add distributed caching
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsapp");
app.UseSession();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
