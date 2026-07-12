using System.Net.Http.Headers;
using System.Security.Cryptography;
using DependencyInjection;

public interface INotificationService
{
    void Notify();
    Guid GetId();
}

public class NotificationService : INotificationService
{
    private Guid ID = Guid.NewGuid();

    public void Notify()
    {
        Console.WriteLine("Notification send to user@gmail.com");
    }
    public Guid GetId()
    {
        return ID;
    }
}

public interface IPhonecallService
{
    void Call();
}

public class PhonecallService : IPhonecallService
{
    public void Call()
    {
        Random random = new Random();
        System.Console.WriteLine($"Calling number {random.Next()}");
    }
}

public interface IScopedService
{
    Guid ID{get;}
}

public class ScopedService : IScopedService
{
    public Guid ID{get;} = Guid.NewGuid();
}