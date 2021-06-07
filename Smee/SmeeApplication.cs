using McMaster.Extensions.CommandLineUtils;
using System;

namespace Smee
{
  [HelpOption(Inherited = true)]
  public class SmeeApplicationBase : CommandLineApplication
  {

    public string HooksPath { get; set; } = ".git-hooks";
    [Option("-r|--repo-path", "An absolute directory path to the git repository root", CommandOptionType.SingleValue, Inherited = true), DirectoryExists]
    public string RepoPath { get; set; } = null;

    [Option("--overwrite", "By default this command wont overwrite a git hook if it already exists. if passed this will allow overwriting", CommandOptionType.NoValue, Inherited = true)]
    public bool Overwrite { get; set; } = false;


    public string GitRepo => String.IsNullOrEmpty(RepoPath) ? GitUtil.ResovleGitPathFromCurrentDir() : RepoPath;
  }

  [Subcommand(
    typeof(InitCommand),
    typeof(AddCommand))]
  public class SmeeApplication : SmeeApplicationBase
  {
    public static int Main(string[] args)
        => Execute<SmeeApplicationBase>(args);
  }
}
