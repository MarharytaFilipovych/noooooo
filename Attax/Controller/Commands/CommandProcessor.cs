namespace Controller.Commands;

using System;
using System.Collections.Generic;
using System.Linq;

public class CommandProcessor
{
    private readonly List<ICommandHandler> _handlers;

    public CommandProcessor()
    {
        _handlers = new List<ICommandHandler>();
    }

    public void RegisterHandler(ICommandHandler handler)
    {
        _handlers.Add(handler ?? throw new ArgumentNullException(nameof(handler)));
    }

    public bool ProcessCommand(string input)
    {
        var parts = input.Trim().ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        if (parts.Length == 0)
            return true;

        var handler = _handlers.FirstOrDefault(h => h.CanHandle(parts[0]));
        
        if (handler != null)
            return handler.Execute(parts);

        return true;
    }
}