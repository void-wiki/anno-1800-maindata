using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace imgconv
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length < 2)
      {
        Console.WriteLine("Usage: dotnet imgconv.dll <src> <dest>");
        return;
      }

      var src = args[0];
      var dest = args[1];

      // This can't convert weired color to correct
      // var bm = new Bitmap(src);
      // bm.Save(dest, ImageFormat.Png);

      // This can convert correctly, WHY???
      using (var fs = new FileStream(src, FileMode.Open))
      {
        using (var bm = new Bitmap(Image.FromStream(fs)))
        {
          bm.Save(dest, ImageFormat.Png);
        }
      }
    }
  }
}
