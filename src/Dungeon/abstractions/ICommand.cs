namespace Dungeon.abstractions;

public interface ICommand<T>
{
    bool CanExecute(T target);
    void Execute(T target);
}