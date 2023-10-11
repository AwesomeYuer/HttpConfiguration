using Microshaoft;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

var configurationUrl = "https://eamiscstorageacc001.blob.core.windows.net/misc-001/misc.settings.json?sp=r&st=2023-10-11T09:03:31Z&se=2023-10-11T17:03:31Z&sv=2022-11-02&sr=b&sig=cdQzSaieRHL7B8v6MWsVHEWjlLRMEEW3d2TnMGuotsE%3D";

using (var stream = await configurationUrl.AsUrlHttpGetContentReadAsStreamAsync())
{
    configuration.AddJsonStream(stream);
}

string a = configuration.GetValue<string>(nameof(a));
Console.WriteLine(a);


var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton(configurationUrl);

services.AddSingleton(configuration);


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
