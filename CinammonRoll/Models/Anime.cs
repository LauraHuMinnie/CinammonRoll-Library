using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace CinammonRoll.Models
{
    public class Anime
    {

        public string title;
        public string imageurl;
        public Anime(string title, string imageurl)
        {
            setTitle(title);
            setImage(imageurl);
        }

        public void setTitle(string title) { this.title = title; }

        public void setImage(string imageurl) { this.imageurl = imageurl; }
    }

    public class Episode
    {
        public StorageFile episodeFile;
        public string episodeTitle;
        public int episodeNum;
        public string isHere;

        public Episode(StorageFile file, int episodeNum)
        {
            this.episodeTitle = file.DisplayName;
            this.episodeFile = file;
            this.episodeNum = episodeNum;
            this.isHere = "";
        }

        public void MarkEpisode()
        {
            this.isHere = "HERE";
        }

        public void UnmarkEpisode()
        {
            this.isHere = "";
        }
    }

    public class AnimeManager
    {
        public static List<Anime> GetAnimes()
        {
            var animes = new List<Anime>();
            animes.Add(new Anime("Kill la Kill", "Assets/killlakill_poster.jpg"));
            animes.Add(new Anime("Shinsekai Yori", "Assets/shinsekaiyori_poster.jpg"));
            animes.Add(new Anime("Land of the Lustrous", "Assets/landofthelustrous_poster.jpg"));
            animes.Add(new Anime("Vinland Saga", "Assets/vinlandsaga_poster.jpg"));
            animes.Add(new Anime("Tsuki ga Kirei", "Assets/tsukigakirei_poster.jpg"));
            animes.Add(new Anime("Promare", "Assets/promare_poster.jpg"));
            animes.Add(new Anime("Uchouten Kazoku", "Assets/uchoutenkazoku_poster.jpg"));
            animes.Add(new Anime("Akira", "Assets/akira_poster.jpg"));
            animes.Add(new Anime("5 Centimeters per Second", "Assets/5centimeterspersecond_poster.jpg"));
            animes.Add(new Anime("Grand Blue", "Assets/grandblue_poster.jpg"));
            animes.Add(new Anime("Gurren Lagann", "Assets/gurrenlagann_poster.jpg"));
            animes.Add(new Anime("I want to eat your pancreas", "Assets/iwanttoeatyourpancreas_poster.jpg"));
            animes.Add(new Anime("Perfect Blue", "Assets/perfectblue_poster.jpg"));
            animes.Add(new Anime("Planetes", "Assets/planetes_poster.jpg"));
            animes.Add(new Anime("Relife", "Assets/relife_poster.jpg"));
            animes.Add(new Anime("Run with the Wind", "Assets/runwiththewind_poster.jpg"));
            animes.Add(new Anime("Steins;Gate", "Assets/steinsgate_poster.jpg"));
            return animes;
        }

        public static List<Anime> GetQueue()
        {
            var animes = new List<Anime>();
            animes.Add(new Anime("Kill la Kill", "Assets/killlakill.jpg"));
            animes.Add(new Anime("Tsuki ga Kirei", "Assets/tsukigakirei.jpg"));
            animes.Add(new Anime("Vinland Saga", "Assets/vinlandsaga.png"));
            animes.Add(new Anime("Shinsekai Yori", "Assets/shinsekaiyori.jpg"));
            animes.Add(new Anime("Houseki no Kuni", "Assets/landofthelustrous.png"));
            animes.Add(new Anime("Promare", "Assets/promare.png"));
            return animes;
        }
    }
}
