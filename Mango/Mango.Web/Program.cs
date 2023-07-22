using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Injeção do Http (IHttpClientFactory)
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Injeção do Http nos serviços
builder.Services.AddHttpClient<ICouponService, CouponService>();

// Injeção de dependências:
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();

// Para inserir as URLs salvas no appsettings nas variáveis em StaticDetails
StaticDetails.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
