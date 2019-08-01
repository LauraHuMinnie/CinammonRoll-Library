using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using System.Windows.Input;

namespace CinammonRoll.Models
{
    public class Torrent
    {
        public string torrentName;
        public string torrentMagnetLink;
        public string date;

        public Torrent(string name, string magnet, string date)
        {
            this.torrentName = name;
            this.torrentMagnetLink = magnet;
            this.date = date;
        }
    }

    public class NyaaTorrent : Torrent
    {
        private string nyaaStart = "https://nyaa.si/download/";
        private string nyaaEnd = ".torrent";
        
        public ICommand DownloadTorrent { get; set; }

        public NyaaTorrent(string name, string magnet, string date) : base(name, magnet, date)
        {
            string processedString = preprocessId(magnet);
            this.torrentMagnetLink = this.nyaaStart + processedString + this.nyaaEnd;
            DownloadTorrent = new RelayCommand(() =>
            {
                launchTorrent();
            });
        }

        private string preprocessId(string code)
        {
            int index = code.Length - 1;
            for (int i = code.Length-1; i > -1; i--)
            {
                if(Char.IsDigit(code[i]))
                {
                    index = i;
                } else
                {
                    break;
                }
            }
            return code.Substring(index);
        }

        private async void launchTorrent()
        {
            Uri newUri = new Uri(torrentMagnetLink);
            var success = await Windows.System.Launcher.LaunchUriAsync(newUri);
        }
    }

    public class HorribleTorrent : Torrent
    {
        public ICommand DownloadTorrent { get; set; }
        public HorribleTorrent(string name, string magnet, string date) : base(name, magnet, date)
        {
            this.torrentMagnetLink = "magnet:?xt=urn:btih:" + magnet + "&tr=http://nyaa.tracker.wf:7777/announce&tr=udp://tracker.coppersurfer.tk:6969/announce&tr=udp://tracker.internetwarriors.net:1337/announce&tr=udp://tracker.leechersparadise.org:6969/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://open.stealth.si:80/announce&tr=udp://p4p.arenabg.com:1337/announce&tr=udp://mgtracker.org:6969/announce&tr=udp://tracker.tiny-vps.com:6969/announce&tr=udp://peerfect.org:6969/announce&tr=http://share.camoe.cn:8080/announce&tr=http://t.nyaatracker.com:80/announce&tr=https://open.kickasstracker.com:443/announce";
            DownloadTorrent = new RelayCommand(() =>
            {
                launchTorrent();
            });
        }

        private async void launchTorrent()
        {
            Uri newUri = new Uri(torrentMagnetLink);
            var success = await Windows.System.Launcher.LaunchUriAsync(newUri);
        }
    }
}
