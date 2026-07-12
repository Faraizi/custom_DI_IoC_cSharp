using DependencyInjection;

var services = new CustomServiceCollection();

services.AddTransient<IPhonecallService, PhonecallService>();
services.AddScoped<IScopedService,ScopedService>();
services.AddSingleton<INotificationService,NotificationService>();

var serviceProvider = services.BuildServiceProvider();

var singletonService1 = serviceProvider.GetRequiredService<INotificationService>();
var singletonService2 = serviceProvider.GetRequiredService<INotificationService>();

var transientService1 = serviceProvider.GetRequiredService<IPhonecallService>();
var transientService2 = serviceProvider.GetRequiredService<IPhonecallService>();



singletonService1.Notify();
Console.WriteLine("One ID for all:" + " "+singletonService1.GetId());
Console.WriteLine("One ID for all:" + " "+singletonService2.GetId());

transientService1.Call();
transientService2.Call();

using (var scope1 = serviceProvider.CreateScope())
{
    var s1 = scope1.ServiceProvider.GetRequiredService<IScopedService>();
    var s2 = scope1.ServiceProvider.GetRequiredService<IScopedService>();
    Console.WriteLine($"Scope 1, call 1 -> {s1.ID}");
    Console.WriteLine($"Scope 1, call 2 -> {s2.ID}");
}

using (var scope2 = serviceProvider.CreateScope())
{
    var s3 = scope2.ServiceProvider.GetRequiredService<IScopedService>();
    Console.WriteLine($"Scope 2, call 1 -> {s3.ID}");
}