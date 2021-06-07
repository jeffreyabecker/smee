using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Smee
{

  public class AddCommandOptions : GitRepoOptions
  {
    private readonly CommandArgument _hooksToCreateArg;
    private readonly CommandOption _scriptTypeOption;

    public AddCommandOptions(CommandLineApplication application) : base(application)
    {
      _hooksToCreateArg = application.Argument("hook-name(s)", "(Required) The name of the git hook to create " + GetHookNamesDesc() + ", or 'all'", true);
      _hooksToCreateArg.IsRequired();
      _hooksToCreateArg.Accepts(b =>
      {
        b.Values(true, Hooks.AllHooks.Concat(new[] { "all" }).ToArray());
      });
      _scriptTypeOption = application.Option("-t|--script-type", "The type of script/scripting engine to configure. 'ps' - Powershell. 'csx' a dotnet-script C# script. ", CommandOptionType.SingleValue);
      _scriptTypeOption.Accepts(b =>
      {
        b.Values(true, "ps", "csx");
      });
    }

    private static string GetHookNamesDesc() => String.Join(", ", Hooks.AllHooks.Select(x => $"'{x}'"));

    public TargetScriptType Type => _scriptTypeOption.Value()?.ToLowerInvariant() == "csx" ? TargetScriptType.CSharpScript : TargetScriptType.Powershell;


    public IEnumerable<string> HooksToCreate => _hooksToCreateArg.Values.Any(v => v.ToLowerInvariant() == Hooks.All) ? Hooks.AllHooks : _hooksToCreateArg.Values.Select(v => v.ToLower()).Where(v => Hooks.AllHooks.Contains(v));
  }

}