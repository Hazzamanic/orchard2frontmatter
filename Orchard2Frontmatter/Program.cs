using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Orchard2Frontmatter
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);
            var doc = XDocument.Load(Path.Combine(path, "data", "export"));
            var posts = new List<Post>();
            

            foreach(var blogPost in doc.Descendants("Data").Descendants("BlogPost"))
            {
                if (blogPost.Attribute("Status").Value != "Published")
                    continue;



                posts.Add(new Post
                {
                    Title = blogPost.Element("TitlePart").Attribute("Title").Value,

                });
            }
        }

        class Post
        {
            public string Title { get; set; }
            public DateTime Created { get; set; }
            public string Body { get; set; }
            public string TagsRaw { get; set; }
            public string[] Tags => TagsRaw?.Split(',') ?? new string[0];
        }
    }
}
