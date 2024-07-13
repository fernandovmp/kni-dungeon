namespace Dungeon.abstractions;

public interface ICommand<T>
{
    void Execute(T target);
}