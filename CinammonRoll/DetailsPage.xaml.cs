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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CinammonRoll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailsPage : Page
    {
        private static SolidColorBrush INCOMPLETE_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 245, 169, 76));
        private static SolidColorBrush WATCHING_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 97, 171, 255));
        private static SolidColorBrush COMPLETE_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 67, 204, 124));
        private static SolidColorBrush DROPPED_COLOR = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 92, 92));
        private static string INCOMPLETE_TEXT = "Incomplete";
        private static string WATCHING_TEXT = "Watching";
        private static string COMPLETE_TEXT = "Completed";
        private static string DROPPED_TEXT = "Dropped";
        private Series selectedSeries;

        public DetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SeriesDetails s = (SeriesDetails)e.Parameter;
            this.selectedSeries = s.getSeries();
            AnimeTitle.Text = s.getTitle();
            AnimePoster.Source = s.poster;
            DetailsBackground.ImageSource = s.panel;
            EpisodeList.ItemsSource = new List<Episode>(s.getEpisodes());
            SetWatchStatusPanel(s.seriesWatchState);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Episode episode = (Episode)e.ClickedItem;
            int episodeNum = episode.episodeNum;
            this.selectedSeries.currentEpisode = episodeNum;
            this.selectedSeries.UpdateSeriesData();
            Frame.Navigate(typeof(Player), episode);
        }

        private void SetWatchStatusPanel(SeriesState s)
        {
            switch(s)
            {
                case SeriesState.Incomplete:
                    WatchStatusPanel.Background = INCOMPLETE_COLOR;
                    WatchStatusText.Text = INCOMPLETE_TEXT;
                    break;
                case SeriesState.Watching:
                    WatchStatusPanel.Background = WATCHING_COLOR;
                    WatchStatusText.Text = WATCHING_TEXT;
                    break;
                case SeriesState.Complete:
                    WatchStatusPanel.Background = COMPLETE_COLOR;
                    WatchStatusText.Text = COMPLETE_TEXT;
                    break;
                case SeriesState.Dropped:
                    WatchStatusPanel.Background = DROPPED_COLOR;
                    WatchStatusText.Text = DROPPED_TEXT;
                    break;
            }
        }

        // TODO - Update the view files within the series folder. Update the episode count.

        private void DropButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSeriesData(SeriesState.Dropped);
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSeriesData(SeriesState.Complete);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateSeriesData(SeriesState.Watching);
        }

        private void UpdateSeriesData(SeriesState state)
        {
            this.selectedSeries.setWatchState(state);
            this.selectedSeries.UpdateSeriesData();
            this.SetWatchStatusPanel(state);
        }
    }
}
