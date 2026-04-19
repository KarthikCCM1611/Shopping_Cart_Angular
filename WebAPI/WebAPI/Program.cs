using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITodoRepository, FileTodoRepository>();
//builder.Services.AddSingleton<TodoServiceClass>();
//builder.Services.AddSingleton<IToDo, TodoServiceClass>();
builder.Services.AddScoped<IToDo, TodoServiceClass>();
builder.Services.AddScoped<IShoppingCart, ShoppingCartService>();
builder.Services.AddScoped<IMaster, MasterService>();
// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
//    options.JsonSerializerOptions.PropertyNamingPolicy = null;
//    //options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
//});
builder.Services.AddCors(opts =>
    opts.AddPolicy("AllowOrigins",
        p => p.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("AllowOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
