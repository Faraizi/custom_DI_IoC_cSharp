namespace DependencyInjection;

public interface IServiceScope : IDisposable
{
    ServiceProvider ServiceProvider { get; }
}

class ServiceScope : IServiceScope
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceScope(ServiceProvider scopedProvider)
    {
        ServiceProvider = scopedProvider;
    }

    public void Dispose()
    {
    }
}