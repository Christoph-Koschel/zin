using Zin.Editor.Input;

namespace Zin.Editor;

public abstract class EditorMode
{
    public abstract string DisplayName { get; }

    protected readonly ZinEditor Editor;

    public EditorMode(ZinEditor editor)
    {
        Editor = editor;
    }

    public abstract void HandleInput(InputChar input);

    public virtual bool GetStatusText(out string text)
    {
        text = null;
        return false;
    }
}