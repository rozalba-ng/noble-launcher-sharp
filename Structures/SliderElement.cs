using System.Windows.Media.Imaging;

namespace NobleLauncher.Structures
{
    public struct SliderElement
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public BitmapImage Image { get; set; }
        public SliderElement(string Name, string Link, string Image) {
            this.Name = Name.Trim();
            this.Image = new BitmapImage();
            this.Image.BeginInit();
            this.Image.UriSource = new System.Uri($"pack://application:,,,/NobleLauncher;component/{Image}");
            this.Image.EndInit();
            this.Link = Link;
        }
    }
}
