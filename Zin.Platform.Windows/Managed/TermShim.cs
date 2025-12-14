using System.Runtime.InteropServices;

namespace Zin.Platform.Windows.Managed;

internal static class TermShim
{
    [DllImport("termshim", EntryPoint = "term_enter_raw_mode")]
    public static extern bool EnterRawMode();

    [DllImport("termshim", EntryPoint = "term_exit_raw_mode")]
    public static extern bool ExitRawMode();

    [DllImport("termshim", EntryPoint = "term_read")]
    public static extern bool Read(out byte c);

    [DllImport("termshim", EntryPoint = "term_write", CharSet = CharSet.Ansi)]
    public static extern bool Write([MarshalAs(UnmanagedType.LPStr)] string buf, int count);
}
