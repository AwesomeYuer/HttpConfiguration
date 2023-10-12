using Microshaoft;
using Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configurationManager = builder.Configuration;

var configurationUrl = "http://localhost:5195/misc.settings.remote.json";

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// 一般情况 如下方式既可 settings.json 不在本 WebApi Server 下
// configurationManager.AddJsonHttpGet(configurationUrl!);

services.AddSingleton
                (
                    (x) =>
                    {
                        // 由于测试用 settings.json 也在本 WebApi Server 下
                        // 所以回调延迟加载
                        configurationManager.AddJsonHttpGet(configurationUrl!);
                        return configurationUrl;
                    }
                );

services.AddSingleton<IConfigurationBuilder>(configurationManager);

// 隐式 Options 注入
// https://www.zhihu.com/tardis/zm/art/265292938?source_id=1005
services.Configure<MiscSettings>(configurationManager.GetSection(MiscSettings.SectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// settings.json 也在本 WebApi Server 下
app.UseFileServer(true);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
