using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserInformationApp.Contracts;
using UserInformationApp.Infrastructure;
using UserInformationApp.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

builder.Services.AddHttpClient<ApiClient>();
builder.Services.AddTransient<IApiClient, ApiClient>();
builder.Services.AddTransient<IUser, UserService>();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var userService = scope.ServiceProvider.GetRequiredService<IUser>();
Console.WriteLine("Enter page no:");
int page = Convert.ToInt32(Console.ReadLine());

try
{
    var users = await userService.GetAllUsersAsync(page);
    foreach (var user in users)
        Console.WriteLine($"User Id : {user.Id}, User Name: {user.First_Name} {user.Last_Name}, User email : {user.Email}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}


Console.WriteLine("Enter user id :");
int userId = Convert.ToInt32(Console.ReadLine());
try
{
    var user = await userService.GetUserByIdAsync(userId);
    if (user == null)
        Console.WriteLine("No data!");
    else
        Console.WriteLine($"{user.Id}: {user.First_Name} {user.Last_Name} ({user.Email})");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Console.Read();

