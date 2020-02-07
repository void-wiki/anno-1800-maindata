using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace configconv
{
  class Templates
  {
    private static void Walk(List<XElement> list, string dir, Dictionary<string, XElement> dict, XElement elem)
    {
      if (elem.Name == "Template")
      {
        list.Add(elem);

        var name = elem.Element("Name").Value;
        var path = Path.Combine(dir, name);
        dict.Add(path, elem);

        return;
      }

      if (elem.Name == "Group")
      {
        var name = elem.Element("Name").Value;
        dir = Path.Combine(dir, name);
      }

      foreach (var child in elem.Elements())
      {
        Walk(list, dir, dict, child);
      }
    }

    public static async Task Convert(string src, string dest)
    {
      var file = Path.Combine(src, "templates.xml");
      var content = await File.ReadAllTextAsync(file);
      var doc = XDocument.Parse(content);

      var list = new List<XElement>();
      var dict = new Dictionary<string, XElement>();
      Walk(list, "templates", dict, doc.Root);

      await VElement.SaveJson(
        Path.Combine(dest, "templates.json"),
        list.Select(el => new VElement(el)).ToList()
      );
      await VElement.SaveJson(
        Path.Combine(dest, "templates-groups.json"),
        dict.Keys.Select(k => k.Replace('\\', '/'))
      );
      await Task.WhenAll(dict.Select(kvp => VElement.SaveXElement(
        Path.Combine(dest, $"{kvp.Key}.xml"),
        kvp.Value
      )));

      Console.WriteLine($"Templates: {list.Count}");
    }
  }
}
