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
    public sealed partial class QueuePage : Page
    {
        //public List<Anime> animes;
        private List<Series> animes;
        public QueuePage()
        {
            this.InitializeComponent();
            //this.animes = AnimeManager.GetQueue();
            if(App.lib.Count > 0)
            {
                this.animes = App.lib[0].GetSeries(SeriesState.Watching);
                SeriesQueue.ItemsSource = this.animes;
            }
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Player));
        }
    }
}
