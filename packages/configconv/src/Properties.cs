using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace configconv
{
  class Properties
  {
    private static void Walk(XElement element, string upstream, Dictionary<string, XElement> dict)
    {
      if (element.Name == "DefaultValues" || element.Name == "DefaultContainerValues")
      {
        return;
      }

      if (element.Name == "Group")
      {
        var name = element.Element("Name");

        var defaultValues = element.Element("DefaultValues");
        var defaultContainerValues = element.Element("DefaultContainerValues");

        var property = new XElement("Property");
        property.Add(name, defaultValues, defaultContainerValues);
        upstream = Path.Combine(upstream, name.Value);

        dict.Add(upstream, property);
      }

      foreach (var child in element.Elements())
      {
        Walk(child, upstream, dict);
      }
    }

    public static async Task Convert(string src, string dest)
    {
      var file = Path.Combine(src, "properties.xml");
      var content = await File.ReadAllTextAsync(file);
      var doc = XDocument.Parse(content);

      var dict = new Dictionary<string, XElement>();
      Walk(doc.Root, "properties", dict);

      await VElement.SaveJson(
        Path.Combine(dest, "properties.json"),
        new VElement(doc.Root)
      );
      await Task.WhenAll(dict.Select(kvp => VElement.SaveXElement(
        Path.Combine(dest, $"{kvp.Key}.xml"),
        kvp.Value
      )));
    }
  }
}
