using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace configconv
{
  class Assets
  {
    private static void Walk(List<XElement> list, Dictionary<string, XElement> dict, XElement elem)
    {
      if (elem.Name == "Asset")
      {
        list.Add(elem);

        var template = elem.Element("Template")?.Value ?? "_";
        var guid = elem.Element("Values").Element("Standard").Element("GUID").Value;

        dict.Add(Path.Combine(template, guid), elem);

        return;
      }
      foreach (var child in elem.Elements())
      {
        Walk(list, dict, child);
      }
    }

    public static async Task Convert(string src, string dest)
    {
      var file = Path.Combine(src, "assets.xml");
      var content = await File.ReadAllTextAsync(file);
      var doc = XDocument.Parse(content);

      var list = new List<XElement>();
      var dict = new Dictionary<string, XElement>();
      Walk(list, dict, doc.Root);

      await Task.WhenAll(list
        .Select(xe => new VElement(xe))
        .Select((ve, i) => (i: i, v: ve))
        .GroupBy(ivp => ivp.i / 1024)
        .Select(g => g.Select(ivp => ivp.v))
        .Select((l, i) => VElement.SaveJson(
          Path.Combine(dest, $"assets_{i.ToString().PadLeft(4, '0')}.json"),
          l
        ))
      );

      await Task.WhenAll(dict.Select(kvp => VElement.SaveXElement(
        Path.Combine(dest, "assets", $"{kvp.Key}.xml"),
        kvp.Value
      )));

      Console.WriteLine($"Assets: {list.Count}");
    }
  }
}
