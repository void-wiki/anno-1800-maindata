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
    public static async Task Convert(string src, string dest)
    {
      var file = Path.Combine(src, "properties.xml");
      var content = await File.ReadAllTextAsync(file);
      var doc = XDocument.Parse(content);

      await VElement.SaveJson(
        Path.Combine(dest, "properties.json"),
        doc.Root.Elements().Select(el => new VElement(el)).ToList()
      );
    }
  }
}
