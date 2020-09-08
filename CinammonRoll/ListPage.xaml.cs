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
    /// 

    public sealed partial class ListPage : Page
    {
        private readonly int ITEM_WIDTH = 200;
        private readonly int ITEM_HEIGHT = 300;

        public ListPage()
        {
            this.InitializeComponent();
            SearchBar.Visibility = Visibility.Collapsed;
            setupGrid();
            HeaderText.Text = "All";
        }

        private async void setupGrid()
        {
            ListRing.IsActive = true;
            ImportLibraryNotice.Visibility = Visibility.Collapsed;
            if (App.lib.Count > 0)
            {
                await App.lib[0].getSubfolders();
                //await App.lib[0].SetupSearch();
                //SearchBar.Visibility = Visibility.Visible;
            }
            else
            {
                ListRing.IsActive = false;
                ImportLibraryNotice.Visibility = Visibility.Visible;
            }
            setupWatchData();
            setupSearch();
        }

        private async void setupSearch()
        {
            if(App.lib.Count > 0)
            {
                await App.lib[0].SetupSearch();
                ListRing.IsActive = false;
                SearchBar.Visibility = Visibility.Visible;
            }
        }

        private async void setupWatchData()
        {
            if(App.lib.Count > 0)
            {
                await App.lib[0].SetupWatchData();
                ListRing.IsActive = false;
                AnimeGrid.ItemsSource = App.lib[0].collectSeries();
            }
            return;
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "All";
            if (App.lib.Count > 0)
            {
                AnimeGrid.ItemsSource = App.lib[0].collectSeries();
            }
        }

        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Completed";
            if(App.lib.Count > 0)
            {
                AnimeGrid.ItemsSource = App.lib[0].GetSeries(SeriesState.Complete);
            }
        }

        private void Planning_Click(object sender, RoutedEventArgs e)
        {
            HeaderText.Text = "Planning";
            if (App.lib.Count > 0)
            {
                AnimeGrid.ItemsSource = App.lib[0].GetSeries(SeriesState.Incomplete);
            }
        }

        private void HeaderText_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Series s = (Series)e.ClickedItem;
            SeriesDetails sd = s.getSeriesDetails();
            Frame.Navigate(typeof(DetailsPage), sd);
        }

        private void AnimeSearch_TextChange(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //sender.ItemsSource = dataset;
                //List<string> result = App.lib[0].SearchAnime(sender.Text);
                //sender.ItemsSource = result;
                List<Series> animeResult = App.lib[0].AnimeResult(sender.Text);
                AnimeGrid.ItemsSource = animeResult;
            }
        }

        private void AnimeSearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            var selectedItem = args.SelectedItem.ToString();
            sender.Text = selectedItem;
        }

        private void AnimeSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                sender.Text = args.ChosenSuggestion.ToString();
            }
        }

        // TODO: Fix GridView resizing when window gets resized.
        // Links:
        // https://stackoverflow.com/questions/50967701/uwp-xaml-load-dynamic-columns-and-rows-to-gridview
        // https://stackoverflow.com/questions/40565599/get-window-size
        // https://stackoverflow.com/questions/41139535/gridview-item-dynamic-width-according-to-screen-size-in-uwp

        private void onGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var columns = Math.Ceiling(ActualWidth/ITEM_WIDTH);
            var rows = Math.Ceiling(ActualHeight / ITEM_HEIGHT);
            ((ItemsWrapGrid)AnimeGrid.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
            //((ItemsWrapGrid)AnimeGrid.ItemsPanelRoot).ItemHeight = e.NewSize.Height / rows;
        }
    }
}
