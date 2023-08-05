using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inje��o do Http (IHttpClientFactory)
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Inje��o do Http nos servi�os
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>();
builder.Services.AddHttpClient<IOrderService, OrderService>();

// Inje��o de depend�ncias:
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Configura��o para realizar o login na aplica��o
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        // Caso tente acessar um controller sem a autoriza��o correta, ser� redirecionado para o LoginPath
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";
    });

// Para inserir as URLs salvas no appsettings nas vari�veis em StaticDetails
StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
StaticDetails.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];
StaticDetails.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
StaticDetails.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
StaticDetails.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"];

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
