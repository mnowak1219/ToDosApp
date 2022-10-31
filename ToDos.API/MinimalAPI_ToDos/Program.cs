var builder = WebApplication.CreateBuilder(args);
var authenticationSettings = new AuthenticationSettings();

// Binding
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddSingleton<IToDoService, ToDoService>();
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(ToDoValidator));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", policyBuilder =>
    {
        policyBuilder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(builder.Configuration["AllowedOrigins"]);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("FrontEndClient");
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.RegisterToDoEndpoints();
app.RegisterAccountEndpoints();
app.Run();




