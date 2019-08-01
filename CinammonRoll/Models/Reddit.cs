using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CinammonRoll.Models
{
    public class Reddit
    {
        public string postTitle;
        public string url;
        public ICommand OpenThread { get; set; }
        
        public Reddit(string title, string url)
        {
            this.postTitle = title;
            this.url = preprocessString(url);
            OpenThread = new RelayCommand(() =>
            {
                openThread();
            });
        }

        private string preprocessString(string node)
        {
            string front = "https://www.reddit.com/r/anime/comments/"+node.Substring(3)+"/";
            string end = "/";
            char[] checkarr = this.postTitle.ToCharArray();
            List<char> finalarr = new List<char>();

            foreach(Char c in checkarr)
            {
                if (Char.IsLetter(c) || Char.IsDigit(c))
                {
                    finalarr.Add(Char.ToLower(c));
                } else if(Char.IsWhiteSpace(c))
                {
                    finalarr.Add('_');
                }
            }
            string processed = "";
            foreach(Char c in finalarr)
            {
                processed = processed + c;
            }
            return front+processed+end;
        }

        private async void openThread()
        {
            Uri newUri = new Uri(this.url);
            var success = await Windows.System.Launcher.LaunchUriAsync(newUri);
        }
    }
}
