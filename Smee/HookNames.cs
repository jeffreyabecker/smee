namespace Smee
{
  public static class HookNames
  {
    public const string PreCommit = "pre-commit";
    public const string PrepareCommitMsg = "prepare-commit-msg";
    public const string CommitMsg = "commit-msg";
    public const string PostCommit = "post-commit";
    public const string PreRebase = "pre-rebase";
    public const string PostRewrite = "post-rewrite";
    public const string PostCheckout = "post-checkout";
    public const string PostMerge = "post-merge";
    public const string PrePush = "pre-push";
    public const string PreAutoGc = "pre-auto-gc";
    public const string All = "all";

    public static readonly string[] AllHooks = new[] { "pre-commit", "prepare-commit-msg", "commit-msg", "post-commit", "pre-rebase", "post-rewrite", "post-checkout", "post-merge", "pre-push", "pre-auto-gc" };

  }

}