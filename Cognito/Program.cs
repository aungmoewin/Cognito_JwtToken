using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager Configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddCors(o => o.AddDefaultPolicy(builder =>
   builder.AllowCredentials()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .SetIsOriginAllowed((x) => true)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // options.RequireHttpsMetadata = false;
                    //options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        SignatureValidator = (token, _) => new JwtSecurityToken(token),
                        //                        RoleClaimType = "scope",
                        NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        RoleClaimTypeRetriever = (token, _) =>
                           (token as JwtSecurityToken).Claims.Any(c => c.Type == "cognito:groups") ? "cognito:groups" : "scope",


                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ValidateLifetime = true,
                        //ClockSkew = TimeSpan.Zero,
                    };
                });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "cognito:groups" && c.Value == "Admin") || context.User.HasClaim(c => c.Type == "scope" && c.Value == "CVX/agency")));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Configure the HTTP request pipeline.

app.Run();
