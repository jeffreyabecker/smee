using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Smee
{
  public static class UnixUtil
  {
    public static void MarkFileExecutable(string file)
    {
      if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        using (var proc = Process.Start("/bin/bash", $"-c \"chmod +x {file}\""))
        {
          proc.WaitForExit();
        }
      }
    }
  }
}
