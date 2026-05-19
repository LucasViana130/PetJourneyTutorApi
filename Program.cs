using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetJourneyTutorApi.Data;
using PetJourneyTutorApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<TutorService>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<ReminderService>();
builder.Services.AddScoped<ClinicService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PetJourney Tutor API",
        Version = "v1",
        Description = "API RESTful para a jornada do tutor no sistema PetJourney/Clyvo Vet."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
