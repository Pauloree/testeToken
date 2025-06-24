using JwtBearer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();
builder.Services.AddScoped<TokenService>();

var app = builder.Build();

app.MapGet("/", (TokenService service) => service.Generate(new JwtBearer.Models.User(1, "teste@paulo.io", "12345", new [] {
    "student", "premium"
})));

app.Run();
