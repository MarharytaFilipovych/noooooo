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

    private object Resolve(Type type, HashSet<Type>? resolving = null)
    {
        resolving ??= [];

        if (!resolving.Add(type))
            throw new InvalidOperationException($"Circular dependency detected for type {type}");

        if (_typesToObjects.TryGetValue(type, out var existing))
        {
            resolving.Remove(type);
            return existing;
        }

        if (type.IsPrimitive || type.IsEnum)
            throw new InvalidOperationException($"Cannot resolve primitive or enum type: {type}");

        if (!_typesToImplementation.TryGetValue(type, out var binding))
            throw new InvalidOperationException($"Type {type} is not registered in the container.");

        var implementationType = binding.ImplementationType;
        var constructor = implementationType.GetConstructors().First();

        var parameters = constructor.GetParameters()
            .Select(p => Resolve(p.ParameterType, resolving))
            .ToArray();

        var instance = constructor.Invoke(parameters);

        if (binding.Scope == Scope.Singleton)
            _typesToObjects[type] = instance;

        resolving.Remove(type);
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