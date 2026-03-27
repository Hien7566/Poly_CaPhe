var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Thêm Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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