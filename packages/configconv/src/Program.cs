using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace configconv
{
  class Program
  {
    static async Task Main(string[] args)
    {
      if (args.Length < 2)
      {
        Console.WriteLine("Usage: dotnet configconv.dll <dataDir> <convertedDir>");
        return;
      }

      var dataDir = args[0];
      var convertedDir = args[1];

      {
        // data/config/export/main/asset
        var assetDirSrc = Path.Combine(dataDir, "config", "export", "main", "asset");
        var assetDirDest = Path.Combine(convertedDir, "config", "export", "main", "asset");

        await Task.WhenAll(new List<Task> {
          Assets.Convert(assetDirSrc, assetDirDest),
          Templates.Convert(assetDirSrc, assetDirDest),
          Properties.Convert(assetDirSrc, assetDirDest)
        });
      }

      {
        // data/config/gui
        var guiDirSrc = Path.Combine(dataDir, "config", "gui");
        var guiDirDest = Path.Combine(convertedDir, "config", "gui");
        await GUI.Convet(guiDirSrc, guiDirDest);
      }
    }
  }
}
