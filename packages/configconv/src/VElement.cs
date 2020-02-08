using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace configconv
{
  class VElement
  {
    public string name { get; set; }

    public Dictionary<string, string> attributes { get; set; }

    public List<VElement> children { get; set; }

    public string value { get; set; }

    public VElement(XElement xElement)
    {
      this.name = xElement.Name.ToString();
      this.attributes = xElement.Attributes().ToDictionary(attr => attr.Name.ToString(), attr => attr.Value);
      this.children = xElement.Elements().Select(c => new VElement(c)).ToList();
      this.value = this.children.Count > 0 ? "" : xElement.Value;
    }

    /// <summary>
    /// Ensure the directory of a file is exists.
    /// </summary>
    /// <param name="file">Path to the file</param>
    /// <returns></returns>
    public static async Task EnsureDirectory(string file)
    {
      await Task.Run(() =>
      {
        var dir = Path.GetDirectoryName(file);
        if (!Directory.Exists(dir))
        {
          Directory.CreateDirectory(dir);
        }
      });
    }

    public static async Task SaveXElement(string path, XElement elem)
    {
      await EnsureDirectory(path);
      var content = elem.ToString();
      await File.WriteAllTextAsync(path, content);
    }

    public static async Task SaveJson(string path, object value)
    {
      await EnsureDirectory(path);
      var options = new JsonSerializerOptions
      {
        WriteIndented = true,
        // Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.BasicLatin)
      };
      var content = JsonSerializer.Serialize(value, options);
      await File.WriteAllTextAsync(path, content);
    }
  }
}
