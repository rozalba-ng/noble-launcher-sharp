using System.Windows.Media.Imaging;

namespace NoblegardenLauncherSharp.Models
{
    public class SliderElementModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public BitmapImage Image { get; set; }

        public SliderElementModel(string Name, string Link, string Image) {
            this.Name = Name.Trim();
            this.Image = new BitmapImage();
            this.Image.BeginInit();
            this.Image.UriSource = new System.Uri($"pack://application:,,,/NoblegardenLauncherSharp;component/{Image}");
            this.Image.EndInit();
            this.Link = Link;
        }
    }
}
