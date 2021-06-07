using McMaster.Extensions.CommandLineUtils;
using System.IO;

namespace Smee
{

  public class InitCommandApp : CommandLineApplication
  {
    private readonly InitCommandOptions _options;

    public InitCommandApp()
    {
      Name = "init";
      HelpOption("-? | -h | --help");
      Description = "Initializes a hooks folder in the local repo";
      _options = new InitCommandOptions(this);
      OnExecute(Exec);
    }

    private int Exec()
    {
      var absoluteHooksFolder = Path.Combine(_options.GitRepo, _options.HooksFolder);
      if(_options.SubModuleRepo != null)
      {
        CloneSubmodule();
        if (!GitUtil.RequiresIndirection)
        {
          GitUtil.RedirectGitHooksFolder(_options.HooksFolder, _options.GitRepo);
        }
        else
        {
          var targetHooksFolder = Path.Combine(_options.GitRepo, _options.HooksFolder);

          foreach (var hook in Hooks.AllHooks)
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
          GitUtil.RedirectGitHooksFolder(_options.HooksFolder, _options.GitRepo);
        }
        else
        {      
          var targetHooksFolder = Path.Combine(_options.GitRepo, _options.HooksFolder);
          foreach (var hook in Hooks.AllHooks)
          {
            ConnectScriptIfExists(targetHooksFolder, hook);
          }
        }
      }


      return 0;
    }
    private readonly TargetScriptType[] _allTypes = new[] { TargetScriptType.Powershell, TargetScriptType.CSharpScript };
    void ConnectScriptIfExists(string  targetHooksFolder, string hook)
    {
      foreach(var type in _allTypes)
      {
        var target = Path.Combine(targetHooksFolder, hook + type.GetExtension());

        if (File.Exists(target))
        {
          GitUtil.WriteIndirectionScript(_options.GitRepo, _options.HooksFolder, hook, type);
        }
      }
    }

    private void CloneSubmodule()
    {
      var info = new System.Diagnostics.ProcessStartInfo();
      info.FileName = "git";
      info.Arguments = $"submodule add {_options.SubModuleRepo} {_options.HooksFolder}";
      info.WorkingDirectory = _options.GitRepo;

      System.Diagnostics.Process.Start(info).WaitForExit();
    }
  }

}