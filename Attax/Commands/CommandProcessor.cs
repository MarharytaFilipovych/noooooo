using Command;
using Commands.CommandDefinition;
using Commands.CommandExecutor;

namespace Commands;

public class CommandProcessor : ICommandProcessor
{
    private readonly Dictionary<string, ICommandDefinition> _commandDefinitions =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<Type, ITypedExecutor> _executors = new();

    private interface ITypedExecutor
    {
        ExecuteResult Execute(ICommand command);
    }

    private sealed class TypedExecutor<TCommand>(ICommandExecutor<TCommand> inner)
        : ITypedExecutor where TCommand : ICommand
    {
        public ExecuteResult Execute(ICommand command) => inner.Execute((TCommand)command);
    }

    public void Register<TCommand>(ICommandDefinition commandDefinition, ICommandExecutor<TCommand> executor)
        where TCommand : ICommand
    {
        _commandDefinitions[commandDefinition.Name] = commandDefinition;
        _executors[typeof(TCommand)] = new TypedExecutor<TCommand>(executor);
    }

    public IReadOnlyList<ICommandDefinition> Commands() => _commandDefinitions.Values.ToList();

    public ExecuteResult ProcessCommand(string[] args, out string? error)
    {
        error = null;
        
        if (!TryValidateInput(args, out error))
            return ExecuteResult.Error;

        var commandName = args[0];

        if (!TryGetDefinition(commandName, out var definition, out error) || 
            !TryParseCommand(definition!, args, out var command, out error) || 
            !TryGetExecutor(command!.GetType(), out var executor, out error))
            return ExecuteResult.Error;

        return executor.Execute(command);
    }

    private static bool TryValidateInput(string[] args, out string? error)
    {
        if (args.Length == 0)
        {
            error = "Empty command! What do you think we need to process?!";
            return false;
        }

        error = null;
        return true;
    }

    private bool TryGetDefinition(string commandName, out ICommandDefinition? definition, out string? error)
    {
        if (!_commandDefinitions.TryGetValue(commandName, out definition))
        {
            error = $"We do not know this command: {commandName}. Type \"help\" for available commands!";
            return false;
        }

        error = null;
        return true;
    }

    private static bool TryParseCommand(ICommandDefinition definition, string[] args, out ICommand? command, out string? error)
    {
        return definition.TryParse(args, out command, out error);
    }

    private bool TryGetExecutor(Type commandType, out ITypedExecutor executor, out string? error)
    {
        if (!_executors.TryGetValue(commandType, out executor!))
        {
            error = $"No executor registered for command: {commandType.Name}!!!";
            return false;
        }

        error = null;
        return true;
    }
}
