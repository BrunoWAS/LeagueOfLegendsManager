using ApiLeague.Data.Repository.Interfaces;
using ApiLeague.Data.Repository;
using ApiLeague.Data;
using ApiLeague.Services.Interfaces;
using ApiLeague.Services;

var builder = WebApplication.CreateBuilder(args);

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Registrando serviços e repositórios
builder.Services.AddTransient<Database>();
builder.Services.AddTransient<ICampeaoRepository, CampeaoRepository>();
builder.Services.AddTransient<IClasseRepository, ClasseRepository>();
builder.Services.AddTransient<IItemRepository, ItemRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();

builder.Services.AddTransient<ICampeaoService, CampeaoService>();
builder.Services.AddTransient<IClasseService, ClasseService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IItemRecommendationService, ItemRecommendationService>();
builder.Services.AddHttpClient<ISyncService, SyncService>();
builder.Services.AddHttpClient<ISyncItemService, SyncItemService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
