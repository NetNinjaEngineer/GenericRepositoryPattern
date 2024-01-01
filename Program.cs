using ApplyingGenericRepositoryPattern.DAL;
using ApplyingGenericRepositoryPattern.Data;
using ApplyingGenericRepositoryPattern.Handlers.Helpers;
using ApplyingGenericRepositoryPattern.Repository;
using ApplyingGenericRepositoryPattern.Repository.Implementation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped(typeof(GPAProvider));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton(serviceProvider =>
{
    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    return new ApplicationDbContext(optionsBuilder.Options);
});

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
