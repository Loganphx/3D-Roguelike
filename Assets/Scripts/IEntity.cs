public interface IEntity
{
    public IComponent[] Components { get; }
}public interface IState
{
}

public interface IComponent<T> where T : IState
{
    public T State { get; }
}

public interface IComponent
{
    public void OnFixedUpdate();
}