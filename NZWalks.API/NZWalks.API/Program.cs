using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Aggiunto per il progetto NZWalks
builder.Services.AddDbContext<NZWalksDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalks"));
});

//Services injection (inietto il servizio di Dependency injection)
//Aggiunto per il progetto NZWalks
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
//Aggiunto per il progetto NZWalks
builder.Services.AddScoped<IWalkRepository, WalkRepository>();
//Aggiunto per il progetto NZWalks
builder.Services.AddScoped<IWalkDifficultyRepository, WalkDifficultyRepository>();

//Aggiunto per il progetto NZWalks
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
