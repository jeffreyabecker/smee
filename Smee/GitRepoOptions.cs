using McMaster.Extensions.CommandLineUtils;

namespace Smee
{
  public abstract class GitRepoOptions
  {
    private readonly CommandOption _hooksPathOption;
    private readonly CommandOption _gitPathOption;
    private string _gitDir;
    private readonly CommandOption _overwriteOption;
    private bool _initialzied = false;

    public GitRepoOptions(CommandLineApplication application)
    {
      _hooksPathOption = application.Option("-h|--hooks-path", "A directory relative to the repository root which will contain hooks. Defaults to '.git-hooks'", CommandOptionType.SingleValue);
      _gitPathOption = application.Option("-r|--repo-path", "An absolute directory path to the git repository root", CommandOptionType.SingleValue);
      _gitPathOption.Accepts(b => b.ExistingDirectory());
      _overwriteOption = application.Option("--overwrite", "By default this command wont overwrite a git hook if it already exists. if passed this will allow overwriting", CommandOptionType.NoValue);

    }
    protected virtual void Init()
    {
      _gitDir = _gitPathOption.HasValue() ? _gitPathOption.Value() : GitUtil.ResovleGitPathFromCurrentDir();

    }



    protected void Ensure()
    {
      if (!_initialzied)
      {
        _initialzied = true;
        Init();
      }
    }
    public bool Overwrite => _overwriteOption.HasValue();
    public string GitRepo
    {
      get
      {
        Ensure();
        return _gitDir;
      }
    }
    public string HooksFolder => _hooksPathOption.Value() ?? ".git-hooks";
  }

}