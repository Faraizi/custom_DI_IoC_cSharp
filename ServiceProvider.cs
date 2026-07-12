using DependencyInjection;
using System;

public class ServiceProvider
{
    private readonly IReadOnlyList<ServiceDescriptor> _serviceDescriptors;
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly Dictionary<Type, object> _scopedServices = new();
    private readonly object _lock = new object();
    public ServiceProvider(IReadOnlyList<ServiceDescriptor> serviceDescriptors)
    {
        _serviceDescriptors = serviceDescriptors;
    }

    private ServiceProvider(IReadOnlyList<ServiceDescriptor> serviceDescriptors, Dictionary<Type, object> singletons)
    {
        _serviceDescriptors = serviceDescriptors;
        _singletons = singletons;
    }
    public IServiceScope CreateScope()
    {
        var scopedProvider = new ServiceProvider(_serviceDescriptors, _singletons);
        return new ServiceScope(scopedProvider);
    }

    public T GetRequiredService<T>()
    {
        return (T)GetService(typeof(T));
    }

    public object GetService(Type serviceType)
    {
        var descriptor = _serviceDescriptors.FirstOrDefault(x => x.ServiceType == serviceType)
        ?? throw new Exception($"The service {serviceType.Name} isn't registered");

        return descriptor.Lifetime switch
        {
            ServiceLifetime.Transient => CreateInstance(descriptor.ImplementationType),
            ServiceLifetime.Singleton => CreateSingleton(descriptor.ImplementationType),
            ServiceLifetime.Scoped => CreateScoped(descriptor.ImplementationType),
            _ => throw new NotImplementedException($"{descriptor.ImplementationType.Name} was not registered")
        };
    }

    public object CreateInstance(Type implType)
    {
        var ctor = implType.GetConstructors();
        var firstConstructor = ctor.FirstOrDefault()
             ?? throw new Exception($"No constructor found for {implType.Name}");

        var deps = firstConstructor.GetParameters()
            .Select(p => GetService(p.ParameterType))
            .ToArray();
        return Activator.CreateInstance(implType, deps)!;
    }
    private object CreateSingleton(Type implType)
    {
        if (_singletons.TryGetValue(implType, out var existing))
        {
            return existing;
        }

        lock (_lock)
        {
            if (_singletons.TryGetValue(implType, out existing))
            {
                return existing;
            }

            object newInstance = CreateInstance(implType);
            _singletons[implType] = newInstance;

            return newInstance;
        }
    }

    private object CreateScoped(Type implType)
    {
        if (_scopedServices.TryGetValue(implType, out var existing))
            return existing;

        lock (_scopedServices)
        {
            if (_scopedServices.TryGetValue(implType, out existing))
                return existing;

            var instance = CreateInstance(implType);
            _scopedServices[implType] = instance;
            return instance;
        }
    }
}