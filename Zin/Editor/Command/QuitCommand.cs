namespace Zin.Editor.Command;

public sealed class QuitCommand : ICommand
{
    public string Execute(ZinEditor editor, string[] args, bool forced)
    {
        if (!forced && editor.Content.OnceModified)
        {
            return "Content was modified. Either save it or force exit it";
        }

        editor.Stop();
        return "";
    }
}