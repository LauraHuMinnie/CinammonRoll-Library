using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CinammonRoll.Models;
using Windows.Web.Syndication;
using Windows.Web;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CinammonRoll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public List<NyaaTorrent> nyaaTorrents;
        public List<HorribleTorrent> horribleTorrents;
        public List<Reddit> redditThreads;

        private const string HORRIBLE_URL = "http://www.horriblesubs.info/rss.php?res=1080";
        private const string SUBREDDIT_URL = "https://www.reddit.com/r/anime.rss";
        private const string NYAA_URL = "https://nyaa.si/?page=rss";
        private Uri uri;
        private SyndicationFeed currentNyaaFeed = null;
        private SyndicationFeed currentHorribleFeed = null;
        private SyndicationFeed currentRedditFeed = null;

        public HomePage()
        {
            this.InitializeComponent();
            this.getNyaaTorrents();
            this.getHorribleTorrents();
            this.getRedditThreads();
        }

        public void getNyaaTorrents()
        {
            this.nyaaTorrents = new List<NyaaTorrent>();
            updateNyaaFeed();
        }

        public void getHorribleTorrents()
        {
            this.horribleTorrents = new List<HorribleTorrent>();
            updateHorribleFeed();
        }

        public void getRedditThreads()
        {
            this.redditThreads = new List<Reddit>();
            updateRedditFeed();
        }

        private async void updateNyaaFeed()
        {
            if (!Uri.TryCreate(NYAA_URL.Trim(), UriKind.Absolute, out uri))
            {
                StatusMessage.Text = "Error: Invalid Nyaa URI";
                return;
            }
            SyndicationClient client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;

            client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            this.uri = new Uri(NYAA_URL);
            this.currentNyaaFeed = await client.RetrieveFeedAsync(this.uri);
            DisplayNyaaFeed();
        }

        private void DisplayNyaaFeed()
        {
            foreach(SyndicationItem item in currentNyaaFeed.Items)
            {
                string t_title = item.Title.Text;
                string t_url = item.Id.ToString();
                string t_date = item.PublishedDate.ToString();
                NyaaTorrent t = new NyaaTorrent(t_title, t_url, t_date);
                this.nyaaTorrents.Add(t);
            }
            NyaaRssFeed.ItemsSource = this.nyaaTorrents;
        }

        private async void updateHorribleFeed()
        {
            if(!Uri.TryCreate(HORRIBLE_URL.Trim(), UriKind.Absolute, out uri))
            {
                StatusMessage.Text = "Error: Invalid HorribleSub URI";
                return;
            }
            SyndicationClient client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;
            client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            this.uri = new Uri(HORRIBLE_URL);
            this.currentHorribleFeed = await client.RetrieveFeedAsync(this.uri);
            DisplayHorribleFeed();
        }

        private void DisplayHorribleFeed()
        {
            foreach (SyndicationItem item in currentHorribleFeed.Items)
            {
                string t_title = item.Title.Text;
                string t_url = item.Id.ToString();
                string t_date = item.PublishedDate.ToString();
                HorribleTorrent t = new HorribleTorrent(t_title, t_url, t_date);
                this.horribleTorrents.Add(t);
            }
            HorribleRssFeed.ItemsSource = this.horribleTorrents;
        }

        private async void updateRedditFeed()
        {
            if (!Uri.TryCreate(SUBREDDIT_URL.Trim(), UriKind.Absolute, out uri))
            {
                StatusMessage.Text = "Error: Invalid Reddit URI";
                return;
            }
            SyndicationClient client = new SyndicationClient();
            client.BypassCacheOnRetrieve = true;
            client.SetRequestHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            this.uri = new Uri(SUBREDDIT_URL);
            this.currentRedditFeed = await client.RetrieveFeedAsync(this.uri);
            DisplayRedditFeed();
        }

        private void DisplayRedditFeed()
        {
            foreach (SyndicationItem item in currentRedditFeed.Items)
            {
                string t_title = item.Title.Text;
                string t_url = item.Id.ToString();
                Reddit t = new Reddit(t_title, t_url);
                this.redditThreads.Add(t);
            }
            RedditRssFeed.ItemsSource = this.redditThreads;
        }
    }
}
