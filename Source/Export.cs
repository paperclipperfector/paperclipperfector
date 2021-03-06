
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace PaperclipPerfector
{
    public static class Export
    {
        public static void Run()
        {
            Directory.CreateDirectory("export");

            foreach (var post in Db.Instance.ReadAllPosts(Db.PostState.Posted, int.MaxValue, Db.LimitBehavior.All))
            {
                var massagedXml = post.html.Replace("&euro;", "€").Replace("&mdash;", "—").Replace("&ndash;", "–");
                var doc = new XDocument(
                    new XElement("post",
                            new XElement("author", post.author),
                            new XElement("date", post.creation),
                            new XElement("link", post.link),
                            new XElement("title", post.flavorTitle.Trim('.')),
                            new XElement("body", XElement.Parse(massagedXml))
                        )
                    );
                File.WriteAllText($"export/{post.author}-{post.creation.ToString("yyyyMMddHHmmss")}.xml", doc.ToString());
            }
        }
    }
}
