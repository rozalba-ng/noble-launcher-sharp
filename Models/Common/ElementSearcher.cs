using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Models
{
    class ElementSearcher
    {
        private readonly ContentControl SearchContext;

        private readonly Dictionary<string, FrameworkElement> CachedElements = new Dictionary<string, FrameworkElement>();
        private readonly Dictionary<string, Storyboard> CachedStories = new Dictionary<string, Storyboard>();

        public ElementSearcher(ContentControl SearchContext) {
            this.SearchContext = SearchContext;
        }

        public FrameworkElement FindName(string name) {
            if (CachedElements.ContainsKey(name)) {
                return CachedElements[name];
            }

            CachedElements[name] = (FrameworkElement)SearchContext.FindName(name);
            return CachedElements[name];
        }

        public Storyboard FindStoryboard(string name) {
            if (CachedStories.ContainsKey(name)) {
                return CachedStories[name];
            }

            CachedStories[name] = (Storyboard)SearchContext.FindResource(name);
            return CachedStories[name];
        }
    }
}
