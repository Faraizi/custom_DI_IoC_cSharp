namespace DependencyInjection;

class CustomServiceCollection()
{
    private readonly List<ServiceDescriptor> _services = new();

    public void AddTransient<TServiceType, TImplementationType>()
        where TServiceType : class
        where TImplementationType : class, TServiceType
    {
        _services.Add(new ServiceDescriptor(
         typeof(TServiceType),
        typeof(TImplementationType),
        ServiceLifetime.Transient
        ));

    }

    public void AddSingleton<TServiceType, TImplementationType>()
    where TServiceType : class
    where TImplementationType : class, TServiceType
    {
        _services.Add(new ServiceDescriptor(
         typeof(TServiceType),
        typeof(TImplementationType),
        ServiceLifetime.Singleton
        ));

    }

        public void AddScoped<TServiceType, TImplementationType>()
    where TServiceType : class
    where TImplementationType : class, TServiceType
    {
        _services.Add(new ServiceDescriptor(
         typeof(TServiceType),
        typeof(TImplementationType),
        ServiceLifetime.Scoped
        ));

    }

    public ServiceProvider BuildServiceProvider()
    {
        return new ServiceProvider(_services.AsReadOnly());
    }
}