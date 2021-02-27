namespace NoblegardenLauncherSharp.Models
{
    public class SingleNewsModel
    {
        public string Title{ get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public AuthorModel Author { get; set; }

        public SingleNewsModel(string title, string content, string link, string authorName, string authorLink) {
            Title = title;
            Content = content;
            Link = link;
            Author = new AuthorModel(authorName, authorLink);
        }
    }
}
