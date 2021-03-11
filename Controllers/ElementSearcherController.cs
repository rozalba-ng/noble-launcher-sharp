using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Controllers
{
    class ElementSearcherController
    {
        private static ElementSearcherController instance;
        private readonly MainWindow Window;

        private readonly Dictionary<string, FrameworkElement> CachedElements = new Dictionary<string, FrameworkElement>();
        private readonly Dictionary<string, Storyboard> CachedStories = new Dictionary<string, Storyboard>();

        private ElementSearcherController(MainWindow Window) {
            this.Window = Window;
        }

        public static ElementSearcherController Init(MainWindow Window) {
            if (instance == null) {
                instance = new ElementSearcherController(Window);
            }

            return instance;
        }

        public static ElementSearcherController GetInstance() {
            return instance;
        }

        public FrameworkElement FindName(string name) {
            if (CachedElements.ContainsKey(name)) {
                return CachedElements[name];
            }

            CachedElements[name] = (FrameworkElement)Window.FindName(name);
            return CachedElements[name];
        }

        public Storyboard FindStoryboard(string name) {
            if (CachedStories.ContainsKey(name)) {
                return CachedStories[name];
            }

            CachedStories[name] = (Storyboard)Window.FindResource(name);
            return CachedStories[name];
        }
    }
}
