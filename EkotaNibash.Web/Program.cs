using EkotaNibash.DataAccess.Managers;
using EkotaNibash.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(180);
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<BoniyadiContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BoniyadiDb")));


builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddRadzenComponents();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

#region Services

builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<IDashboard, DashboardManager>();
builder.Services.AddScoped<IAccount, AccountManager>();
builder.Services.AddScoped<ICompany, CompanyManager>();
builder.Services.AddScoped<IExpense, ExpenseManager>();
builder.Services.AddScoped<ILookup, LookupManager>();

builder.Services.AddScoped<IEkotaMember, EkotaMemberManager>();
builder.Services.AddScoped<INominee, NomineeManager>();
builder.Services.AddScoped<IPayment, PaymentManager>();
builder.Services.AddScoped<IMemberDocument, MemberDocumentManager>();

#endregion Services

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();