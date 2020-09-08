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
using System.Diagnostics;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CinammonRoll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QueuePage : Page
    {
        //public List<Anime> animes;
        private List<SeriesQueue> animes;
        private SeriesDetails selectedSeriesDetails;
       
        public QueuePage()
        {
            this.InitializeComponent();
            //this.animes = AnimeManager.GetQueue();
            if(App.lib.Count > 0)
            {
                this.animes = App.lib[0].GetSeriesQueue(SeriesState.Watching);
                SeriesQueue.ItemsSource = this.animes;
            }
            this.DetailsColumn.Visibility = Visibility.Collapsed;
        }

        private async void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Player));
            Series selectedSeries = this.selectedSeriesDetails.getSeries();
            Episode ep = selectedSeries.getCurrentEpisode();

            // Start video player process
            /*Process p = new Process();
            p.EnableRaisingEvents = false;
            p.StartInfo.FileName = ep.episodeFile.Path;
            p.Start();*/
            var promptOptions = new Windows.System.LauncherOptions()
            {
                DisplayApplicationPicker = true
            };
            var success = await Windows.System.Launcher.LaunchFileAsync(ep.episodeFile, promptOptions);
            if (success)
            {

            } else
            {
                var messageDialog = new MessageDialog("Failed to load Application picker for the episode file " + ep.episodeTitle);
                messageDialog.Commands.Add(new UICommand(
                    "Close",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;
                await messageDialog.ShowAsync();
            }
        }

        private void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DetailsPage), this.selectedSeriesDetails);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Series s = (Series)e.ClickedItem;
            this.selectedSeriesDetails = s.getSeriesDetails();
            this.DisplayPoster.Source = s.poster;
            this.DetailsColumn.Visibility = Visibility.Visible;
        }

        private void CommandInvokedHandler(IUICommand command)
        {

        }
    }
}
