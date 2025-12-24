using System.Collections.Generic;

namespace Zin.Editor.Command;

public sealed class CommandHandler
{
    private Dictionary<string, ICommand> _commands;

    public CommandHandler()
    {
        _commands = new Dictionary<string, ICommand>();
    }

    public void RegisterCommand(string prefix, ICommand command)
    {
        _commands[prefix] = command;
    }

    public string Execute(ZinEditor editor, string commandString)
    {
        string[] args = commandString.Split(' ');
        bool forced = args[0][^1] == '!';
        string prefix = forced
            ? args[0][..^1]
            : args[0];

        if (!_commands.TryGetValue(prefix, out ICommand command))
        {
            return "Unknown command";
        }

        return command.Execute(editor, args, forced);
    }

    public static CommandHandler Default()
    {
        // TODO this section should be ported to a javascript configuration file
        CommandHandler handler = new CommandHandler();

        handler.RegisterCommand(":o", new OpenCommand());
        handler.RegisterCommand(":q", new QuitCommand());

        return handler;
    }
}