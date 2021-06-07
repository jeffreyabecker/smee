using McMaster.Extensions.CommandLineUtils;
using System;
namespace Smee
{

  public static class Program
  {


    public static int Main(string[] args)
    {
      try
      {
        var app = new CommandLineApplication();
        app.AddSubcommand(new AddCommandApp());
        app.AddSubcommand(new InitCommandApp());
        return app.Execute(args);
      }
      catch (Exception e)
      {
        if (e is AggregateException aggregateEx)
        {
          e = aggregateEx.Flatten().InnerException;
        }
        Console.Error.WriteLine(e.ToString());
        return 1;
      }
    }
  }

}