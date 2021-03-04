namespace NoblegardenLauncherSharp.Models
{
    public class AuthorModel
    {
        public string Name { get; set; }
        public string Link { get; set; }

        public AuthorModel(string name, string link) {
            Name = name.Trim();
            Link = link;
        }
    }
}
