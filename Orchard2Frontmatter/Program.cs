using System;
using System.Linq;
using System.Xml.Linq;

namespace Orchard2Frontmatter
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = XDocument.Load(@"C:\Users\break\Source\Repos\Orchard2Frontmatter\Orchard2Frontmatter\data\export.xml");
            Console.WriteLine(doc.Descendants("Data").Descendants("BlogPost").FirstOrDefault().Element("TitlePart").Attribute("Title").Value);
        }
    }
}
