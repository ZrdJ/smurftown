using System.IO;

namespace Smurftown;

public abstract class Directories
{
    public static readonly string UserPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".smurftown");
}