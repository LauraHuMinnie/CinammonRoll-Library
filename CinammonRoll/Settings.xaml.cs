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
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using CinammonRoll.Models;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CinammonRoll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        public Settings()
        {
            this.InitializeComponent();
            DirectoryList.ItemsSource = App.lib;
        }

        private void AddDirButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFolder();
        }

        private void RemoveDirButton_Click(object sender, RoutedEventArgs e)
        {
            int[] arr = new int[DirectoryList.SelectedItems.Count];
            for (int i = 0; i < DirectoryList.SelectedItems.Count; i++)
            {
                var selectedIndex = DirectoryList.Items.IndexOf(DirectoryList.SelectedItems[i]);
                arr[i] = selectedIndex;
            }
            List<Library> t = new List<Library>(App.lib);
            for(int i = arr.Length-1; i > -1; i--)
            {
                t.RemoveAt(i);
            }
            App.lib = t;
            DirectoryList.ItemsSource = App.lib;
        }

        /*private async void getSeries(int index)
        {
            await this.lib[index].getSubfolders();
            //StatusMessage.Text = this.lib[0].getFirstSeries();
        }*/

        private async void OpenFolder()
        {
            var folderOpenPicker = new FolderPicker();
            folderOpenPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderOpenPicker.FileTypeFilter.Add("*");

            ProgressRing.IsActive = true;
            StorageFolder f = await folderOpenPicker.PickSingleFolderAsync();
            if (f != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", f);
                StatusMessage.Text = "Folder picked";
                Library l = new Library(f);
                List<Library> temp = new List<Library>(App.lib);
                temp.Add(l);
                //App.lib.Add(l);
                ProgressRing.IsActive = false;
                App.lib = temp;
                DirectoryList.ItemsSource = App.lib;
            } else
            {
                StatusMessage.Text = "Cancelled";
                ProgressRing.IsActive = false;
            }
        }
    }
}
