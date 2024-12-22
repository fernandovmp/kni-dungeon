namespace Dungeon.abstractions;

public static class CommandExtensions
{
    public static bool TryExecute<T>(this ICommand<T> command, T parameter)
    {
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
            return true;
        }
        return false;
    }
    
    
    public static bool TryExecute<T>(this T parameter, ICommand<T> command)
    {
        return command.TryExecute(parameter);
    }
}