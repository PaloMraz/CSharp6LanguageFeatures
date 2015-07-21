using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;

namespace CSharp6LanguageFeatures
{
  class Program
  {
    // Dictionary index initializers.
    private static Dictionary<Type, string> _s_typeNamesOld = new Dictionary<Type, string>()
    {
      { typeof(int), "int" },
      { typeof(string), "string" },
      { typeof(bool), "bool" }
    };
    private static Dictionary<Type, string> _s_typeNamesNew = new Dictionary<Type, string>()
    {
      [typeof(int)] = "int",
      [typeof(string)] = "string",
      [typeof(bool)] = "bool"
    };


    static void Main(string[] args)
    {
      // Null-conditional operator.

      string name = null;
      var nameLength = name?.Length;
      List<string> names = null;
      nameLength = names?[0].Length;

      var notifier = new Notifier();
      notifier.ChangeState();
      notifier.StateChanged += (sender, e) => WriteLine("StateChanged notification received...");
      notifier.ChangeState();

      // await in catch/finally blocks.
      notifier.ChangeStateAsync().Wait();

      // Auto-property initializers + expression-bodied members + string interpolation.
      var snapshot = new Snapshot();
      WriteLine(snapshot);
      WriteLine($"{snapshot.UserName} created on {snapshot.Timestamp} with name length of {name?.Length ?? 0}");

      // nameof expressions.
      WriteLine(nameof(args));
      WriteLine(nameof(notifier));
      WriteLine(nameof(Main));
      WriteLine(nameof(Program));

      ReadLine();
    }


    private static void AutoPropertyInitializers()
    {
      var snapshot1 = new Snapshot();
      WriteLine(snapshot1);
    }


    public class Notifier
    {
      public event EventHandler StateChanged;

      public void ChangeState()
      {
        this.StateChanged?.Invoke(this, EventArgs.Empty);
      }


      public async Task ChangeStateAsync()
      {
        try
        {
          await Task.Run(() => this.ChangeState());
        }
        catch (Exception ex)
        {
          await this.LogAsync(ex.ToString());
          throw;
        }
        finally
        {
          await this.LogAsync("Leaving ChangeStateAsync...");
        }
      }


      private async Task LogAsync(string message)
      {
        await Task.Run(() => WriteLine("Log: " + message));
      }
    }


    public class Snapshot
    {
      public string UserName { get; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
      public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;

      public override string ToString() => $"{this.UserName} {this.Timestamp}";
    }


  }
}
