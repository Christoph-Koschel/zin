using System.Runtime.InteropServices;

namespace Zin.Platform.Linux.Managed;

internal static partial class TermShim
{
    [LibraryImport("termshim", EntryPoint = "term_enter_raw_mode")]
    public static partial int EnterRawMode();

    [LibraryImport("termshim", EntryPoint = "term_exit_raw_mode")]
    public static partial int ExitRawMode();

    [LibraryImport("termshim", EntryPoint = "term_read")]
    public static partial int Read(out byte c);

    [LibraryImport("termshim", EntryPoint = "term_write", StringMarshalling = StringMarshalling.Utf8)]
    public static partial int Write(string buf, int count);
}