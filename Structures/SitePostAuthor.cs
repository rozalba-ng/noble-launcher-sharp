namespace NobleLauncher.Structures
{
    public struct SitePostAuthor
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public SitePostAuthor(string name, string link) {
            Name = name.Trim();
            Link = link;
        }
    }
}
