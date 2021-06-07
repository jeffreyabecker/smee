using McMaster.Extensions.CommandLineUtils;
using System.IO;

namespace Smee
{

  public class AddCommandApp : CommandLineApplication
  {
    private readonly AddCommandOptions _options;

    public AddCommandApp()
    {
      Name = "add";
      HelpOption("-? | -h | --help");
      Description = "Sets up a new git hook indirection which works on Windows, MacOS and Linux";
      _options = new AddCommandOptions(this);
      OnExecute(Exec);
    }

    private int Exec()
    {

      var targetHooksFolder = Path.Combine(_options.GitRepo, _options.HooksFolder);
      if (!GitUtil.RequiresIndirection)
      {
        GitUtil.RedirectGitHooksFolder(_options.HooksFolder, _options.GitRepo);
      }
      var templateScript = GitUtil.GetTemplateScript(_options.Type);
      foreach (var hook in _options.HooksToCreate)
      {
        var targetFile = Path.Combine(targetHooksFolder, hook + _options.Type.GetExtension());
        if (GitUtil.RequiresIndirection)
        {
          GitUtil.WriteIndirectionScript(_options.GitRepo, _options.HooksFolder, hook, _options.Type);
        }
        if (!File.Exists(targetFile) || _options.Overwrite)
        {
          File.WriteAllText(targetFile, templateScript);
          UnixUtil.MarkFileExecutable(targetFile);
        }

      }
      return 0;
    }


  }

}