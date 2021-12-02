using System.ComponentModel;

namespace Smee
{
  public enum TargetScriptType
  {
    Powershell,
    CSharpScript,
    Shell
  }
  public static class TargetScriptTypeExtensions
  {
    public static string GetExtension(this TargetScriptType type)
    {
      if (type == TargetScriptType.Powershell) return ".ps1";
      if (type == TargetScriptType.CSharpScript) return ".csx";
      if (type == TargetScriptType.Shell) return ".sh";
      throw new InvalidEnumArgumentException($"Unknown Script Type {type}. Did you forget to add the extension?");

    }
  }

}