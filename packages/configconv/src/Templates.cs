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
    private static void Walk(XElement element, string upstream, Dictionary<string, XElement> dict)
    {
      if (element.Name == "Template")
      {
        var name = element.Element("Name").Value;
        var path = Path.Combine(upstream, name);
        dict.Add(path, element);

        return;
      }

      if (element.Name == "Group")
      {
        var name = element.Element("Name").Value;
        upstream = Path.Combine(upstream, name);
      }

      foreach (var child in element.Elements())
      {
        Walk(child, upstream, dict);
      }
    }

    public static async Task Convert(string src, string dest)
    {
      var file = Path.Combine(src, "templates.xml");
      var content = await File.ReadAllTextAsync(file);
      var doc = XDocument.Parse(content);

      var dict = new Dictionary<string, XElement>();
      Walk(doc.Root, "templates", dict);

      await VElement.SaveJson(
        Path.Combine(dest, "templates.json"),
        new VElement(doc.Root)
      );
      await Task.WhenAll(dict.Select(kvp => VElement.SaveXElement(
        Path.Combine(dest, $"{kvp.Key}.xml"),
        kvp.Value
      )));
    }
  }
}
