using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Orchard2Frontmatter
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var directory = System.IO.Path.GetDirectoryName(path);
            var outputDirectory = Path.Combine(path, "output");
            var doc = XDocument.Load(Path.Combine(path, "data", "export.xml"));
            var posts = new List<Post>();

            foreach (var blogPost in doc.Descendants("Data").Descendants("BlogPost"))
            {
                if (blogPost.Attribute("Status").Value != "Published")
                    continue;

                var body = blogPost.Element("BodyPart").Attribute("Text").Value;

                posts.Add(new Post
                {
                    Title = blogPost.Element("TitlePart").Attribute("Title").Value,
                    TagsRaw = blogPost.Element("TagsPart").Attribute("Tags").Value,
                    Alias = blogPost.Element("AutoroutePart").Attribute("Alias").Value,
                    Body = body,
                    PublishedUtc = XmlConvert.ToDateTime(blogPost.Element("CommonPart").Attribute("PublishedUtc").Value, XmlDateTimeSerializationMode.Utc)
                });
            }

            if (Directory.Exists(outputDirectory))
                Directory.Delete(outputDirectory, true);

            Directory.CreateDirectory(outputDirectory);

            foreach (var post in posts)
            {
                var postFilePath = Path.Combine(outputDirectory, post.Alias.Replace('/', '\\') + ".md");
                Console.WriteLine("Creating file: " + postFilePath);

                var fileInfo = new FileInfo(postFilePath);
                if(!Directory.Exists(fileInfo.Directory.FullName)) {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }
                using (FileStream fs = File.Create(postFilePath))
                {
                    // Add some text to file    
                    Byte[] text = new UTF8Encoding(true).GetBytes(post.Body);
                    fs.Write(text, 0, text.Length);
                }

                Console.WriteLine("Created file: " + postFilePath);
            }
        }

        class Post
        {
            public string Title { get; set; }
            public DateTime PublishedUtc { get; set; }
            public string Body { get; set; }
            public string TagsRaw { get; set; }
            public string Alias { get; set; }
            public string[] Tags => TagsRaw?.Split(',') ?? new string[0];
        }
    }
}
