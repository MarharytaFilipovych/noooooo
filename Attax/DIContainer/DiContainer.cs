namespace Core;

public class DiContainer : IDiContainer
{
    private readonly Dictionary<Type, Binding> _typesToImplementation = new();
    
    private readonly Dictionary<Type, object> _typesToObjects = new();
    
    
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
        var binding = _typesToImplementation[type];

        if (binding.Scope == Scope.Singleton && _typesToObjects.TryGetValue(type, out var result))
            return result;
        
        var implementationType = binding.ImplementationType;
        var constructor = implementationType.GetConstructors().First();
        var args = new List<object>();
        
        foreach (var parameter in constructor.GetParameters())
        {
            args.Add(Resolve(parameter.ParameterType));
        }

        var instance = constructor.Invoke(args.ToArray());

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