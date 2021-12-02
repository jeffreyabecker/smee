using McMaster.Extensions.CommandLineUtils;
using System.IO;

namespace Smee
{
  [Command(Name = "init", Description = "Initializes a hooks folder in the local repo")]
  public class InitCommand : SmeeCommand
  {

    public int OnExecute(CommandLineApplication app)
    {
      var absoluteHooksFolder = Path.Combine(GitRepo, HooksPath);
      if (SubModuleRepo != null)
      {
        CloneSubmodule();
        if (!GitUtil.RequiresIndirection)
        {
          GitUtil.RedirectGitHooksFolder(HooksPath, GitRepo);
        }
        else
        {
          var targetHooksFolder = Path.Combine(GitRepo, HooksPath);

          foreach (var hook in HookNames.AllHooks)
          {
            ConnectScriptIfExists(targetHooksFolder, hook);
          }
        }
      }
      else
      {

        if (!Directory.Exists(absoluteHooksFolder))
        {
          Directory.CreateDirectory(absoluteHooksFolder);
        }
        if (!GitUtil.RequiresIndirection)
        {
          GitUtil.RedirectGitHooksFolder(HooksPath, GitRepo);
        }
        else
        {
          var targetHooksFolder = Path.Combine(GitRepo, HooksPath);
          foreach (var hook in HookNames.AllHooks)
          {
            ConnectScriptIfExists(targetHooksFolder, hook);
          }
        }
      }


      return 0;
    }
    private readonly TargetScriptType[] _allTypes = new[] { TargetScriptType.Powershell, TargetScriptType.CSharpScript };
    void ConnectScriptIfExists(string targetHooksFolder, string hook)
    {
      foreach (var type in _allTypes)
      {
        var target = Path.Combine(targetHooksFolder, hook + type.GetExtension());

        if (File.Exists(target))
        {
          GitUtil.WriteIndirectionScript(GitRepo, HooksPath, hook, type);
        }
      }
    }

    private void CloneSubmodule()
    {
      var info = new System.Diagnostics.ProcessStartInfo();
      info.FileName = "git";
      info.Arguments = $"submodule add {SubModuleRepo} {HooksPath}";
      info.WorkingDirectory = GitRepo;

      System.Diagnostics.Process.Start(info).WaitForExit();
    }

    [Option("--from-repo", "A git repo url to clone as a submodule into your hooks folder", CommandOptionType.SingleValue)]
    public string SubModuleRepo { get; set; }
  }

}