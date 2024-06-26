using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using TodoApi.Data;
using TodoApi.Helper;
using TodoApi.Interfaces;
using TodoApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(options=>{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IPokemonInterface, PokemonRepository>();
builder.Services.AddScoped<ICategoryInterface, CategoryRepository>();
builder.Services.AddScoped<ICountryInterface, CountryRepository>();
builder.Services.AddScoped<IOwnerInterface, OwnerRepository>();
builder.Services.AddScoped<IReviewInterface, ReviewRepository>();
builder.Services.AddTransient<Seed>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(args.Length == 1 && args[0].ToLower()=="seeddata"){
    SeedData(app);
}

void SeedData(IHost app){
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using(var scope = scopedFactory.CreateScope()){
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }

}

//middlewares

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
