namespace NoblegardenLauncherSharp.Structures
{
    public struct SitePost
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public SitePostAuthor Author { get; set; }
        public SitePost(string title, string content, string link, string authorName, string authorLink) {
            Title = title.Trim();
            Content = content.Trim();
            Link = link;
            Author = new SitePostAuthor(authorName, authorLink);
        }
    }
}
