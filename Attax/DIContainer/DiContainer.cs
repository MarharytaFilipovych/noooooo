namespace Core;

public class DiContainer : IDiContainer
{
    private readonly Dictionary<Type, Binding> _typesToImplementation = new();
    
    private readonly Dictionary<Type, object> _typesToObjects = new();
    
    public void RegisterInstance<T>(T instance)
    {
        _typesToObjects[typeof(T)] = instance!;
    }

    public void Register<TInterface, TImplementation>(Scope scope) where TImplementation : TInterface
    {
        _typesToImplementation[typeof(TInterface)] = new Binding()
        {
            InterfaceType = typeof(TInterface),
            ImplementationType = typeof(TImplementation),
            Scope = scope
        };
    }

    private object Resolve(Type type)
    {
        if (_typesToObjects.TryGetValue(type, out var existing))
            return existing;
        
        if (type.IsPrimitive || type.IsEnum)
            throw new InvalidOperationException($"Cannot resolve primitive or enum type: {type}");
        
        if (!_typesToImplementation.TryGetValue(type, out var binding))
            throw new InvalidOperationException($"Type {type} is not registered in the container.");
        
        if (binding.Scope == Scope.Singleton && _typesToObjects.TryGetValue(type, out var result))
            return result;
        
        var implementationType = binding.ImplementationType;
        var constructor = implementationType.GetConstructors().First();

        var instance = constructor.Invoke(constructor.GetParameters()
            .Select(parameter => Resolve(parameter.ParameterType)).ToArray());

        _typesToObjects[type] = instance;
        return instance;
    }

    public T Resolve<T>()
    {
        return (T)Resolve(typeof(T));
    }


    private class Binding
    {
        public required Type InterfaceType { get; init; }
        public required Type ImplementationType { get; init; }
        public required Scope Scope { get; init; }
    }
}