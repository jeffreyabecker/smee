using McMaster.Extensions.CommandLineUtils;
using System;

namespace Smee
{
  [HelpOption(Inherited = false)]
  public abstract class SmeeCommand
  {

    [Option("-h|--hooks-path", "A directory relative to the repository root which will contain hooks. Defaults to '.git-hooks'", CommandOptionType.SingleValue)]
    public string HooksPath { get; set; } = ".git-hooks";

    [Option("-r|--repo-path", "An absolute directory path to the git repository root", CommandOptionType.SingleValue, Inherited = false), DirectoryExists]
    public string RepoPath { get; set; } = null;


    public string GitRepo => String.IsNullOrEmpty(RepoPath) ? GitUtil.ResovleGitPathFromCurrentDir() : RepoPath;
  }
}