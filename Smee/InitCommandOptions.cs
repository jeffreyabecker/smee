using McMaster.Extensions.CommandLineUtils;


namespace Smee
{

  public class InitCommandOptions : GitRepoOptions
  {

    private readonly CommandOption _scriptTypeOption;
    private readonly CommandOption _subModuleOption;

    public InitCommandOptions(CommandLineApplication application) : base(application)
    {

      _scriptTypeOption = application.Option("-t|--script-type", "The type of script/scripting engine to configure. 'ps' - Powershell. 'csx' a dotnet-script C# script. ", CommandOptionType.SingleValue);
      _scriptTypeOption.Accepts(b =>
      {
        b.Values(true, "ps", "csx");
      });

      _subModuleOption = application.Option("--from-repo", "A git repo url to clone as a submodule into your hooks folder", CommandOptionType.SingleValue);
    }
    public TargetScriptType Type => _scriptTypeOption.Value().ToLowerInvariant() == "csx" ? TargetScriptType.CSharpScript : TargetScriptType.Powershell;
    public string SubModuleRepo => _subModuleOption.HasValue() ? _subModuleOption.Value() : null;

  }

}