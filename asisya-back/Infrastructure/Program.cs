using Infrastructure.Config;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

/*************************************** CUSTOM ***************************************/
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; }); // No Validaci�n Autom�tica
builder.Services.AddValidatorService();
builder.Services.AddDBContextService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Profile_Mapper));
builder.Services.AddDependencyInjectionService(builder.Configuration);
builder.Services.AddControllerService(); // builder.Services.AddControllers - IgnoreCycles
builder.Services.AddCORSService(builder.Configuration);
/************************************** /CUSTOM/ **************************************/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*************************************** CUSTOM ***************************************/
app.UseRouting();
app.UseCors("AllowFront");
app.UseMiddleware();
app.UseMigrationSeed();
/************************************** /CUSTOM/ **************************************/

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
