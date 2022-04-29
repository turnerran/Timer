using WebApplication1.Services;
using WebApi.Helpers;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ISchedueledTaskService, SchedueledTaskService>();
builder.Services.AddScoped<ITaskActionService, TaskActionService>();
builder.Services.AddScoped<IOnInitService, OnInitService>();

// Add db context and pass in connection string
builder.Services.AddDbContext<DataContext>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var serviceScopeFactory = app.Services.GetService<IServiceScopeFactory>();
using (var scope = serviceScopeFactory.CreateScope())
{
    var handler = (IOnInitService)scope.ServiceProvider.GetService(typeof(IOnInitService));
    await handler.DoOverDueTasks();
}

app.Run();

