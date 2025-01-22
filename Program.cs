using Artemis;
using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.AutoMapper;
using Artemis.Backend.Services.Authentication;
using Artemis.Frontend.Components;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Change this to Inforamtion in Production!

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMvc();
builder.Services.AddControllersWithViews();

// Add Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();

// Configure HTTPS redirection
builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 5001;
});

// Add HttpClient with proper SSL handling
builder.Services.AddHttpClient("Artemis", client =>
{
    var baseUrl = builder.Environment.IsDevelopment()
        ? "http://localhost:5000/"  // API calls use HTTP port 5000
        : "https://localhost:5001/";
    client.BaseAddress = new Uri(baseUrl);
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Database services
bool databaseConnected = true;
try
{
    builder.Services.AddArtemisDatabase(builder.Configuration);
}
catch (ApplicationException ex)
{
    var logger = LoggerFactory.Create(config => config.AddConsole())
        .CreateLogger("Program");
    logger.LogError(ex.InnerException, message: ex?.Message ?? "");
    databaseConnected = false;
    builder.Configuration["databaseConnected"] = databaseConnected.ToString();
}

// Add Transaction Scope
builder.Services.AddScoped<ITransactionScope>(provider =>
{
    var context = provider.GetRequiredService<ArtemisDbContext>();
    return new TransactionScope(context);
});

// Security Settings
// Create and populate SecuritySettings directly
var securitySettings = new SecuritySettings
{
    JwtKey = builder.Configuration["SecuritySettings:JwtKey"] ?? string.Empty,
    JwtIssuer = builder.Configuration["SecuritySettings:JwtIssuer"] ?? string.Empty,
    JwtAudience = builder.Configuration["SecuritySettings:JwtAudience"] ?? string.Empty,
    JwtExpiryInDays = int.TryParse(builder.Configuration["SecuritySettings:JwtExpiryInDays"], out int days) ? days : 1,
    TokenExpirationMinutes = int.TryParse(builder.Configuration["SecuritySettings:TokenExpirationMinutes"], out int expiration) ? expiration : 480,
    MaxFailedAttempts = int.TryParse(builder.Configuration["SecuritySettings:MaxFailedAttempts"], out int attempts) ? attempts : 5,
    LockoutTimeMinutes = int.TryParse(builder.Configuration["SecuritySettings:LockoutTimeMinutes"], out int lockout) ? lockout : 15
};

if (string.IsNullOrEmpty(securitySettings?.JwtKey))
{
    throw new InvalidOperationException("JWT Key must be configured in SecuritySettings");
}

// Register the populated SecuritySettings object with DI
builder.Services.Configure<SecuritySettings>(options =>
{
    options.JwtKey = securitySettings.JwtKey;
    options.JwtIssuer = securitySettings.JwtIssuer;
    options.JwtAudience = securitySettings.JwtAudience;
    options.JwtExpiryInDays = securitySettings.JwtExpiryInDays;
    options.TokenExpirationMinutes = securitySettings.TokenExpirationMinutes;
    options.MaxFailedAttempts = securitySettings.MaxFailedAttempts;
    options.LockoutTimeMinutes = securitySettings.LockoutTimeMinutes;
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = ArtemisAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = ArtemisAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = ArtemisAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = ArtemisAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(ArtemisAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = ArtemisAuthenticationDefaults.CookieName;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;

    // For debugging
    options.Events = new CookieAuthenticationEvents
    {
        OnSigningIn = context =>
        {
            context.Properties.IsPersistent = true;
            context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1);
            return Task.CompletedTask;
        },
        OnSignedIn = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Cookie set for user: {User}", context.Principal?.Identity?.Name);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthentication()
    .AddBearerToken(options =>
    {
        options.BearerTokenExpiration = TimeSpan.FromHours(1);
        options.Events = new BearerTokenEvents
        {
            OnMessageReceived = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILogger<Program>>();

                if (context.Token == null)
                {
                    logger.LogWarning("No token received");
                    context.Response.StatusCode = 401;
                }
                else
                {
                    logger.LogInformation("Token received and validated");
                }

                return Task.CompletedTask;
            }
        };
    });

// Add JWT Configuration
builder.Services.Configure<TokenValidationParameters>(options =>
{
    options.ValidateIssuerSigningKey = true;
    options.IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(securitySettings.JwtKey));
    options.ValidateIssuer = true;
    options.ValidateAudience = true;
    options.ValidateLifetime = true;
    options.ClockSkew = TimeSpan.Zero;  // Remove default 5 min tolerance
    options.ValidIssuer = securitySettings.JwtIssuer;
    options.ValidAudience = securitySettings.JwtAudience;
});

// Add cookie policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
    options.Secure = CookieSecurePolicy.SameAsRequest;
    options.CheckConsentNeeded = _ => false;
});

// Authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsAuthenticatedUser", policy => policy.RequireAuthenticatedUser());

// Antiforgery
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
    options.Cookie.Name = "Artemis.Antiforgery";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.FormFieldName = "__RequestVerificationToken";
});

// Required Services
builder.Services.AddHttpContextAccessor();

// Custom Services Registration
InversionOfControl.AddServiceRegistration(builder.Services);

// Build the application
var app = builder.Build();

/** ** ** ** ** ** */
// Middleware !!!

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
else
{
    app.UseDeveloperExceptionPage();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

// Security headers with HSTS
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    if (!app.Environment.IsDevelopment())
    {
        headers.StrictTransportSecurity = "max-age=31536000; includeSubDomains";
    }
    headers.XContentTypeOptions = "nosniff";
    headers.XFrameOptions = "DENY";
    headers.XXSSProtection = "1; mode=block";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "accelerometer=(), camera=(), geolocation=(), microphone=()";
    await next();
});

// Core middleware (order is important)
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Diagnostic middleware only in Development mode
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogDebug("Request to {Path} - Cookies: {Cookies}",
            context.Request.Path,
            string.Join(", ", context.Request.Cookies.Select(c => c.Key)));
        await next();
    });
}

// Endpoints
app.MapControllers();

if (!databaseConnected)
{
    app.MapFallback(async context =>
    {
        context.Response.Redirect("/Error");
        await Task.CompletedTask;
    });
}
else
{
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
}

app.UseRequestLocalization();

app.Run();