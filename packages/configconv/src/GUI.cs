using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace configconv
{
  class GUI
  {
    public static async Task Convet(string src, string dest)
    {
      var files = await Task.Run(() => Directory.GetFiles(src, "texts_*.xml"));
      await Task.WhenAll(files.Select(async pathXml =>
      {
        var pathJson = Path.Combine(
          dest,
          Path.GetFileName(pathXml).Replace(Path.GetExtension(pathXml), ".json")
        );
        var contentXml = await File.ReadAllTextAsync(pathXml);
        var doc = XDocument.Parse(contentXml);
        var dict = doc.Root.Element("Texts").Elements().ToDictionary(
          text => text.Element("GUID").Value,
          text => text.Element("Text").Value
        );
        await VElement.SaveJson(pathJson, dict);
      }));
    }
  }
}
