namespace Core;

public interface IDiContainer
{
    void Register<TInterface, TImplementation>(Scope scope)
        where TImplementation : TInterface;
    
    T Resolve<T>();
}

public enum Scope
{
    Singleton, Transient
}