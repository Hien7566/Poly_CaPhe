var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<Poly_Cafe.BLL.ShiftBLL>();
// Thêm Session
builder.Services.AddDistributedMemoryCache();
// Sửa đoạn này trong Program.cs
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

    // THÊM 2 DÒNG NÀY ĐỂ FIX LỖI COOKIE TRÊN CHROME/EDGE
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ĐÃ SỬA: Tạm thời tắt tự động chuyển hướng HTTPS để sửa lỗi ERR_CONNECTION_RESET
// app.UseHttpsRedirection(); 

app.UseRouting();
app.UseSession();  // ← THÊM DÒNG NÀY
app.UseAuthorization();
app.MapStaticAssets();

// SỬA: Đổi Home thành Dashboard
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();