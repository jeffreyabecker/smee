using McMaster.Extensions.CommandLineUtils;

namespace Smee
{
  [Command("smee")]
  [Subcommand(
    typeof(InitCommand),
    typeof(AddCommand))]
  public class Smee : SmeeCommand
  {
    public static void Main(string[] args) => CommandLineApplication.Execute<Smee>(args);
    public int OnExecute(CommandLineApplication app)
    {
      app.ShowHelp();
      return 0;
    }
  }
}