using BlogReaderInfrastructureLibrary.Database;
using Microsoft.EntityFrameworkCore;
using BlogReaderCoreLibrary.Interfaces;
using BlogReaderInfrastructureLibrary.Repositories;
using AutoMapper;
using BlogReaderCoreLibrary.Entities;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.SqlServer;
using BlogReaderInfrastructureLibrary.Services;
using BlogReaderSharedLibrary.DTOs;
using BlogReaderCoreSharedLibrary.FirebaseNotifications;
using BlogReaderInfrastructure.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#if DEBUG
    builder.Services.AddDbContext<BlogReaderDBContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));
#else
    builder.Services.AddDbContext<BlogReaderDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RemoteConnection")));
#endif

builder.Services.AddScoped<ISourceRepository, SourceRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IRSSreader, RSSreader>();
FCM.Init();
builder.Services.AddControllersWithViews()
                .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

#if DEBUG
    builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170).UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings().UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireLocalConnection"), 
    new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true,
    }));
#else
    builder.Services.AddHangfire(configuration => configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170).UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings().UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireRemoteConnection"), 
    new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true,
    }));
#endif

builder.Services.AddHangfireServer();
builder.Services.AddHostedService<JobWorker>();

//Registering AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var configuration = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<Source, SourceDTO>();
    cfg.CreateMap<Blog, BlogDTO>();
    cfg.CreateMap<Article, ArticleDTO>();
});
var mapper = configuration.CreateMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHangfireDashboard();

//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard();
});

app.MapControllers();

app.Run();
