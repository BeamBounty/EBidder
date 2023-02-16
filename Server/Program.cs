using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Radzen;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using EBidderWeb.Server.Data;
using Microsoft.AspNetCore.Identity;
using EBidderWeb.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddSingleton(sp =>
{
    // Get the address that the app is currently running at
    var server = sp.GetRequiredService<IServer>();
    var addressFeature = server.Features.Get<IServerAddressesFeature>();
    string baseAddress = addressFeature.Addresses.First();
    return new HttpClient{BaseAddress = new Uri(baseAddress)};
});
builder.Services.AddScoped<EBidderWeb.Server.EBidderService>();
builder.Services.AddDbContext<EBidderWeb.Server.Data.EBidderContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EBidderConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderEBidder = new ODataConventionModelBuilder();
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.Admin>("Admins");
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.Artwork>("Artworks");
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.Buyer>("Buyers").EntityType.HasKey(entity => new
    {
    entity.SSN, entity.BuyerID
    });
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.GameItem>("GameItems");
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.Nft>("Nfts");
    oDataBuilderEBidder.EntitySet<EBidderWeb.Server.Models.EBidder.Seller>("Sellers").EntityType.HasKey(entity => new
    {
    entity.SSN, entity.SellerID
    });
    opt.AddRouteComponents("odata/EBidder", oDataBuilderEBidder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<EBidderWeb.Client.EBidderService>();
builder.Services.AddHttpClient("EBidderWeb.Server").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<EBidderWeb.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("EBidderConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, EBidderWeb.Client.ApplicationAuthenticationStateProvider>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseHeaderPropagation();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToPage("/_Host");
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();