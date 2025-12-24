namespace Zin.Editor.Command;

public interface ICommand
{
    public string Execute(ZinEditor editor, string[] args, bool forced);
}