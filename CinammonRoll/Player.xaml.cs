using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
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
using Windows.Media.Core;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using CinammonRoll.Models;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CinammonRoll
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Player : Page
    {
        private const string FILE_TOKEN = "{CA31C767-E5CF-4DB9-BCC8-A03C2AF99DD4}";
        public Player()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Episode s = (Episode)e.Parameter;
            StorageFile file = s.episodeFile;
            if (file != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(FILE_TOKEN, file);
                if (!App.setOptions)
                {
                    mediaPlayer.Options.Add("avcodec-hw", "d3d11va");
                    App.setOptions = true;
                }
                mediaPlayer.Source = null;
                mediaPlayer.Source = $"winrt://{FILE_TOKEN}";
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private async void OpenFile()
        {
            var fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.FileTypeFilter.Add("*");
            var file = await fileOpenPicker.PickSingleFileAsync();
            if(file != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(FILE_TOKEN, file);
                mediaPlayer.Source = null;
                mediaPlayer.Source = $"winrt://{FILE_TOKEN}";
            }
        }
    }
}
