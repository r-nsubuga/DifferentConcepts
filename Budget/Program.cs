using Budget.Data;
using Budget.Events;
using Budget.Helpers;
using Budget.Repositories;
using Budget.Services;
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
builder.Services.AddMediatR(cfg=> 
    cfg.RegisterServicesFromAssemblies(typeof(BudgetCreatedEventHandler).Assembly));
builder.Services.AddSingleton<ElasticsearchClient>(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
        .Authentication(new BasicAuthentication("elastic", "############"))
        .ServerCertificateValidationCallback((sender, certificate, chain, errors) => true)
        ;
    return new ElasticsearchClient(settings);
});
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();
