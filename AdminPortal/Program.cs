using AdminPortal.Data;
using AdminPortal.Filters;
using AdminPortal.Models;
using AdminPortal.Options;
using AdminPortal.Providers;
using AdminPortal.Services;
using AdminPortal.Services.FilePreview;
using AdminPortal.Services.FileStorage;
using AdminPortal.Services.Lookup;
using Audit.Core;
using Audit.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        // 审计
        options.Filters.Add(new AuditPageFilter()
        {
            IncludeHeaders = true,
            IncludeModel = true

        });

        // 权限，用于action
        options.Filters.Add<AuthorizeHandlerFilter>();
    });

builder.Services.Configure<IdentityOptions>(options =>
{
    // 密码设置
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 4;

    // Lockout settings.
    // 连续输错或5次，锁定10分钟
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.Name = "_AdminPortal_Cookie_";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// 自定义policy
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// Transient：每次从 DI 容器解析（Resolve）时都会创建一个新实例
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.Configure<EmailOption>(builder.Configuration.GetSection("Email"));
builder.Services.AddOptions<AdminOption>().Bind(builder.Configuration.GetSection("RoleDefault")).ValidateDataAnnotations().ValidateOnStart();

builder.Services.AddHostedService<SeedHostedService>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ILookupService, LookupService>();

builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();
builder.Services.AddScoped<IFileService, FileService>();

// 文件预览
builder.Services.AddScoped<IFilePreviewFactory, FilePreviewFactory>();
builder.Services.AddScoped<IFilePreviewService, FilePreviewService>();
builder.Services.AddScoped<IFilePreviewStrategy, TextFilePreviewStrategy>();
builder.Services.AddScoped<IFilePreviewStrategy, ImageFilePreviewStrategy>();
builder.Services.AddScoped<IFilePreviewStrategy, PDFFilePreviewStrategy>();

// 健康检查
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>("Database").AddCheck("self", () => HealthCheckResult.Healthy());

// Audit配置，输出到文件
Audit.Core.Configuration.Setup()
    .UseFileLogProvider(_ => _
        .DirectoryBuilder(_ => Path.Combine(AppContext.BaseDirectory, "AuditLogs"))
        .FilenameBuilder(ev => $"{ev.StartDate:yyyyMMddHHmmssffff}.json"))
    .WithCreationPolicy(EventCreationPolicy.InsertOnEnd);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 健康检查
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false   // 不检查依赖，只检查程序是否存活
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = _ => true    // 检查所有依赖（数据库等）
});

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
