using System.IO;

namespace Zin.Editor.Command;

public sealed class OpenCommand : ICommand
{
    public string Execute(ZinEditor editor, string[] args, bool forced)
    {
        if (args.Length < 2)
        {
            return "Missing file path";
        }

        string filePath = args[1];
        try
        {
            editor.Content.OpenContent(File.ReadAllLines(filePath));
            editor.SetCursorAbsolute(0, 0);
            return string.Empty;
        }
        catch
        {
            return "Failed to open file";
        }
    }
}