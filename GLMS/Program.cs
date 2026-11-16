using GLMS.Data;
using GLMS.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>(); 
builder.Services.AddScoped<FileService>(); 
builder.Services.AddScoped<IContractFactory, ContractFactory>(); 
builder.Services.AddScoped<ICurrencyStrategy, ApiCurrencyStrategy>(); 
builder.Services.AddScoped<CurrencyConverter>(sp =>
    new CurrencyConverter(sp.GetRequiredService<ICurrencyStrategy>())); 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]