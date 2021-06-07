using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Smee
{
  public class GitUtil
  {

    public static string ResovleGitPathFromCurrentDir()
    {
      var initialDir = Directory.GetCurrentDirectory();
      var currentDir = initialDir;
      while (looping(currentDir))
      {
        currentDir = Path.GetDirectoryName(currentDir);
      }
      if (!Directory.Exists(Path.Combine(currentDir, ".git")))
      {
        throw new InvalidOperationException($"The current directory {initialDir} is not part of a git repo");
      }
      return currentDir;
      bool looping(string dir) => Path.GetPathRoot(dir) != dir && !Directory.Exists(Path.Combine(dir, ".git"));
    }
    public static string GetTemplateScript(TargetScriptType type)
    {

      if (type == TargetScriptType.Powershell)
      {
        return "#!/usr/bin/env pwsh\nWrite-Host 'Hello, Smee Hooks!'";
      }
      else if (type == TargetScriptType.CSharpScript)
      {
        return "#!/usr/bin/env dotnet-script\nConsole.WriteLine(\"Hello, Smee Hooks!\");";
      }
      else
      {
        throw new ArgumentException($"script-type must be one of 'ps', 'csx'", "script-type");
      }
    }

    public static void WriteIndirectionScript(string gitRepo, string hooksFolder, string hook, TargetScriptType type)
    {
      var targetGitHook = Path.Combine(gitRepo, ".git", "hooks", hook);

      var shebang = GetWindowsShebang();
      var cmd = GetDelegationCommand(type, Path.Combine(hooksFolder, hook + type.GetExtension()));
      var content = shebang + "\n" + cmd;
      File.WriteAllText(targetGitHook, content, Encoding.ASCII);
      UnixUtil.MarkFileExecutable(targetGitHook);
    }

    public static string GetDelegationCommand(TargetScriptType type, string pathRelativeToRepoRoot)
    {
      if (type == TargetScriptType.Powershell)
      {
        return $"exec powershell.exe -NoProfile -ExecutionPolicy Bypass -File \"{pathRelativeToRepoRoot} $@\"\nexit";
      }
      else if (type == TargetScriptType.CSharpScript)
      {
        return $"exec dotnet-script.exe \"{pathRelativeToRepoRoot}\" -- \"$@\"\nexit";
      }
      else
      {
        throw new ArgumentException($"script-type must be one of 'ps', 'csx'", "script-type");
      }
    }

    public static string GetWindowsShebang()
    {
      var possibleGits = Environment.GetEnvironmentVariable("Path").Split(';').Where(x => x.ToLower().Contains(@"\git\"))
        .Select(x=>x.TrimEnd('\\')).ToList();


      var usrBin = possibleGits.FirstOrDefault(v => v.ToLower().EndsWith(@"\git\usr\bin"));
      if (usrBin != null)
      {
        return "#!" + EscapeWindowsPathForCygwin(usrBin) + "/sh.exe";
      }
      throw new InvalidOperationException("Git for windows is not in your path!");
      string EscapeWindowsPathForCygwin(string source) => source.Replace('\\', '/').Replace(" ", "\\ ").TrimEnd('/');
    }

    public static void RedirectGitHooksFolder(string hooksFolder, string gitRepo)
    {


      using (var p = Process.Start(new ProcessStartInfo
      {
        FileName = "git",
        Arguments = $"config core.hooksPath {hooksFolder}",
        WorkingDirectory = gitRepo
      })) { p.WaitForExit(); }
    }
    private static int[] _gitVersion = null;
    private static int[] GetGitVersion()
    {
      using (var proc = Process.Start(new ProcessStartInfo
      {
        FileName = "git",
        Arguments = "version"
      }))
      {
        proc.WaitForExit();
        var version = proc.StandardOutput.ReadToEnd().Trim().Substring(13);//trim off 'git version '
        return version.Split('.').Take(2).Select(x => Int32.Parse(x)).ToArray();
      }
    }
    public static bool RequiresIndirection
    {
      get
      {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { return true; }
        if (_gitVersion == null)
        {
          _gitVersion = GetGitVersion();
        }
        if (_gitVersion[0] > 2 || (_gitVersion[0] == 2 && _gitVersion[1] >= 9))
        {
          return false;
        }
        return true;

      }
    }
  }
}