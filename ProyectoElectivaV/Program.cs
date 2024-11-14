using AutoMapper;
using Microsoft.Extensions.Options;
using ProyectoElectivaV.Mapeos;
using ProyectoElectivaV.Model.DbConfiguration;
using ProyectoElectivaV.Service;
using ProyectoElectivaV.Service.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProyectoElectivaVDBSettings>(
    builder.Configuration.GetSection(nameof(ProyectoElectivaVDBSettings))
    );

builder.Services.AddSingleton<IProyectoElectivaVDBSettings>
    (d => d.GetRequiredService<IOptions<ProyectoElectivaVDBSettings>>().Value);

builder.Services.AddSingleton<SerieService>();
builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddSingleton<ICorreoService,CorreoService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mapping
var mapperConfig = new MapperConfiguration(m =>
{
    m.AddProfile(new ProyectoMap());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc();


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
