using Budget.Data;
using Budget.Events;
using Budget.Helpers;
using Budget.Repositories;
using Budget.Services.Budgets;
using Budget.Services.MQ;
using Budget.Services.MQ.Publishers;
using Budget.Services.MQ.Subscribers;
using Budget.Services.SignalR;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BConnectionString");
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<BudgetDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>(provider => 
    new RabbitMqService("localhost", "budgets"));
builder.Services.AddTransient<BudgetUpdatePublisher>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<RabbitMqListener>();

builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
            .Authentication(new BasicAuthentication("elastic", "ghfgdfhjvghcfdgfc45"))
            .ServerCertificateValidationCallback((sender, certificate, chain, errors) => true)
        ;
    return new ElasticsearchClient(settings);
});

builder.Services.AddMediatR(cfg=> 
    cfg.RegisterServicesFromAssemblies(typeof(BudgetCreatedEventHandler).Assembly));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Explicitly allow React app
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Required for SignalR with authentication
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseRouting();
app.MapHub<MessageHub>("/messageHub");
app.Run();
