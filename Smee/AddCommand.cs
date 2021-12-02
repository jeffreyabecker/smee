using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smee
{
  [Command(Name = "add", Description = "Sets up a new git hook indirection which works on Windows, MacOS and Linux")]
  public class AddCommand : SmeeCommand
  {

    public int OnExecute(CommandLineApplication app)
    {

      var targetHooksFolder = Path.Combine(GitRepo, HooksPath);
      if (!GitUtil.RequiresIndirection)
      {
        GitUtil.RedirectGitHooksFolder(HooksPath, GitRepo);
      }
      var templateScript = GitUtil.GetTemplateScript(Type);
      foreach (var hook in HooksToCreate)
      {
        var targetFile = Path.Combine(targetHooksFolder, hook + Type.GetExtension());
        if (GitUtil.RequiresIndirection)
        {
          GitUtil.WriteIndirectionScript(GitRepo, HooksPath, hook, Type);
        }
        if (!File.Exists(targetFile) || Overwrite)
        {
          File.WriteAllText(targetFile, templateScript);
          UnixUtil.MarkFileExecutable(targetFile);
        }

      }
      return 0;
    }

    [Option("-t|--script-type", "The type of script/scripting engine to generate. 'ps' - Powershell. 'csx' a dotnet-script C# script. ", CommandOptionType.SingleValue)]
    public string Ext { get; set; }

    [Argument(0, "hook-name(s)", Description = "(Required) The name of the git hook(s) to create  or 'all'"),
      AllowedValues("pre-commit", "prepare-commit-msg", "commit-msg", "post-commit", "pre-rebase", "post-rewrite", "post-checkout", "post-merge", "pre-push", "pre-auto-gc", "all", Comparer = System.StringComparison.InvariantCultureIgnoreCase)]
    public List<string> Hooks { get; set; } = new List<string> { "all" };

    [Option("--overwrite", "Overwrite existing scripts with the generated hook", CommandOptionType.NoValue, Inherited = false)]
    public bool Overwrite { get; set; } = false;

    public TargetScriptType Type => Ext?.ToLowerInvariant() == "csx" ? TargetScriptType.CSharpScript : TargetScriptType.Powershell;

    public IEnumerable<string> HooksToCreate => Hooks.Any(v => v.ToLowerInvariant() == HookNames.All) || Hooks.Count == 0 ? HookNames.AllHooks : Hooks.Select(v => v.ToLower()).Where(v => HookNames.AllHooks.Contains(v));
  }

}