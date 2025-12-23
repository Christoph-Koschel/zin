using Zin.Editor.Input;

namespace Zin.Editor.Mode;

public sealed class CommandMode : EditorMode
{
    public override string DisplayName => "Command";

    public CommandMode(ZinEditor editor) : base(editor)
    {
    }

    public override void HandleInput(InputChar input)
    {

    }
}