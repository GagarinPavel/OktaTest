using Okta.AspNet.Abstractions;
using Okta.AspNetCore;
using OktaWebApiOptions = Okta.AspNetCore.OktaWebApiOptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
    options.AddPolicy(
      "AllowAll",
      builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});


var Configuration = builder.Configuration;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
})
.AddOktaWebApi(new OktaWebApiOptions()
{
    OktaDomain = Configuration["Okta:OktaDomain"],
    AuthorizationServerId = Configuration["Okta:AuthorizationServerId"],
    Audience = Configuration["Okta:Audience"]
});

builder.Services.AddAuthorization();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
