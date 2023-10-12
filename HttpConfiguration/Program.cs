using Microshaoft;
using Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var configuration = builder.Configuration;

var configurationUrl = "http://localhost:5195/misc.settings.remote.json";

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// 一般情况 如下方式既可, 由于 settings.json 不在本 WebApi Server 下
// configuration.AddJsonHttpGet(configurationUrl!);

// 特殊情况 随 configurationUrl 注入,
// 由于测试用 settings.json 也在本 WebApi Server 下,
// 以便 controller 构造延迟首次一次性回调时加载初始配置
services
        .AddSingleton
            (
                (x) =>
                {
                    // 由于测试用 settings.json 也在本 WebApi Server 下
                    // 所以首次一次性回调延迟加载
                    configuration.AddJsonHttpGet(configurationUrl!);
                    return configurationUrl;
                }
            );

//services.AddSingleton<IConfigurationBuilder>(configuration);

// 隐式 Options 注入
// https://www.zhihu.com/tardis/zm/art/265292938?source_id=1005
services.Configure<MiscSettings>(configuration.GetSection(MiscSettings.SectionName));

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
