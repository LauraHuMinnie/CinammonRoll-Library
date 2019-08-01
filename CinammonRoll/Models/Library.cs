using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CinammonRoll.Models
{
    public class Library
    {
        public string directory;
        private StorageFolder libraryDir;
        private List<Series> series;
        private SearchManager searchManager;
        private bool getSubDone;
        private bool getSearchDone;
        private WatchData watchData;

        public Library(StorageFolder dir)
        {
            series = new List<Series>();
            this.libraryDir = dir;
            this.directory = this.libraryDir.Path;
            this.getSubDone = false;
            this.getSearchDone = false;
            //this.getSubfolders();
        }

        public async Task getSubfolders()
        {
            if (!this.getSubDone)
            {
                this.series.Clear();
                IReadOnlyList<StorageFolder> folders = await this.libraryDir.GetFoldersAsync();
                foreach (StorageFolder folder in folders)
                {
                    string folderName = folder.DisplayName;
                    string dir = folder.Path;
                    IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
                    int count = 0;
                    List<string> filenames = new List<string>();
                    List<StorageFile> filepaths = new List<StorageFile>();
                    StorageFile poster = null;
                    StorageFile panel = null;
                    StorageFile data = null;

                    foreach (StorageFile file in files)
                    {
                        if (file.FileType == ".mkv")
                        {
                            count++;
                            filenames.Add(file.DisplayName);
                            filepaths.Add(file);
                        }
                        else if (file.FileType == ".png" || file.FileType == ".jpg")
                        {
                            if (file.DisplayName == "poster")
                            {
                                poster = file;
                            }
                            else if (file.DisplayName == "panel")
                            {
                                panel = file;
                            }
                        }
                        else if (file.FileType == ".dat")
                        {
                            if (file.DisplayName == "view")
                            {
                                data = file;
                            }
                        }
                    }
                    Series s = new Series(folderName, filepaths, filenames, count, dir, data, folder);
                    s.setPanelFile(panel);
                    s.setPosterFile(poster);
                    this.series.Add(s);
                }
                this.getSubDone = true;
            }
            return;
        }

        public async Task SetupWatchData()
        {
            this.watchData = new WatchData();
            foreach(Series s in this.series)
            {
                await s.GetSeriesData();
                this.watchData.AddSeries(s);
            }
            /*
             * The Old way of doing things
            await this.watchData.GetData(this.series);
            int counter = 0;
            foreach(EpisodeData ep in this.watchData.GetSeriesData())
            {
                this.series[counter].setWatchState((SeriesState)ep.watchStatus);
                counter++;
            }
            */
            return;
        }

        public async Task SetupSearch()
        {
            if (!this.getSearchDone)
            {
                this.searchManager = new SearchManager();
                int counter = 0;
                foreach (Series s in this.series)
                {
                    this.searchManager.AddWord(s.getTitle(), counter);
                    counter++;
                }
                this.getSearchDone = true;
            }
            return;
        }

        public List<Series> GetSeries(SeriesState targetState)
        {
            List<Series> result = this.watchData.GetSeriesList(this.series, targetState);
            return result;
        }


        public List<string> SearchAnime(string s)
        {
            List<int> indices = this.searchManager.SearchWord(s);
            List<string> result = new List<string>();
            foreach(int index in indices)
            {
                result.Add(this.series[index].getTitle());
            }
            return result;
        }

        public List<Series> AnimeResult(string s)
        {
            List<int> indices = this.searchManager.SearchWord(s);
            List<Series> result = new List<Series>();
            foreach(int index in indices)
            {
                result.Add(this.series[index]);
            }
            return result;
        }

        public List<Series> collectSeries()
        {
            return new List<Series>(this.series);
        }

        public int totalSeries() { return this.series.Count;  }

        public string getSeriesAt(int index)
        {
            return this.series[index].getTitle();
        }
        
    }

    public class Series
    {

        public string title;
        private int totalEpisodes;
        private List<StorageFile> mediaFilePath;
        private List<string> mediaFileNames;
        private List<Episode> episodes;
        private string directory;
        private StorageFile watchData;
        private StorageFile posterFile;
        private StorageFile panelFile;
        private StorageFolder folder;
        public BitmapImage listposter;
        public BitmapImage poster;
        public BitmapImage panel;
        public SeriesState watchState;
        public BitmapImage watchStateImage;
        private EpisodeData episodeData;
        public int currentEpisode;
        

        public Series(string title, List<StorageFile> filePaths, List<string> fileNames, int count, string directory, StorageFile watchData, StorageFolder folder)
        {
            this.title = title;
            this.mediaFilePath = filePaths;
            this.mediaFileNames = fileNames;
            this.totalEpisodes = count;
            this.directory = directory;
            this.watchData = watchData;
            this.folder = folder;
            setupEpisodes();
        }

        private void setupEpisodes()
        {
            this.episodes = new List<Episode>();
            int counter = 1;
            foreach(StorageFile f in this.mediaFilePath)
            {
                Episode e = new Episode(f, counter);
                this.episodes.Add(e);
                counter++;
            }
        }

        public int getTotalEpisodes()
        {
            return this.totalEpisodes;
        }

        public string getTitle()
        {
            return this.title;
        }

        public void setWatchState(SeriesState s)
        {
            this.watchState = s;
            switch(s)
            {
                case SeriesState.Incomplete:
                    this.watchStateImage = new BitmapImage(new Uri("ms-appx:///Assets/incomplete_banner.png"));
                    break;
                case SeriesState.Watching:
                    this.watchStateImage = new BitmapImage(new Uri("ms-appx:///Assets/watching_banner.png"));
                    break;
                case SeriesState.Complete:
                    this.watchStateImage = new BitmapImage(new Uri("ms-appx:///Assets/completed_banner.png"));
                    break;
                case SeriesState.Dropped:
                    this.watchStateImage = new BitmapImage(new Uri("ms-appx:///Assets/dropped_banner.png"));
                    break;
            }
        }

        public StorageFile getData()
        {
            return this.watchData;
        }

        public StorageFolder getFolder()
        {
            return this.folder;
        }

        public List<Episode> getEpisodes()
        {
            List<Episode> eps = new List<Episode>();
            int counter = 1;
            foreach (Episode e in this.episodes)
            {
                Episode temp = new Episode(e.episodeFile, e.episodeNum);
                if(counter == this.currentEpisode)
                {
                    temp.MarkEpisode();
                }
                eps.Add(temp);
                counter++;
            }
            return eps;
        }

        public List<string> getEpisodeNames()
        {
            return new List<string>(mediaFileNames);
        }

        public StorageFile getFilePath(int index)
        {
            return mediaFilePath[index];
        }

        public void setPosterFile(StorageFile f)
        {
            this.posterFile = f;
            setPoster();
            setListPoster();
        }

        public StorageFile getPosterFile()
        {
            return this.posterFile;
        }

        public void setPanelFile(StorageFile f)
        {
            this.panelFile = f;
            setPanel();
        }

        public StorageFile getPanelFile()
        {
            return this.panelFile;
        }

        private async void setPoster()
        {
            this.poster = new BitmapImage();
            using(var stream = await this.posterFile.OpenReadAsync())
            {
                await this.poster.SetSourceAsync(stream);
            }
        }

        private async void setListPoster()
        {
            this.listposter = new BitmapImage();
            this.listposter.DecodePixelHeight = 300;
            this.listposter.DecodePixelWidth = 200;
            using (var stream = await this.posterFile.OpenReadAsync())
            {
                await this.listposter.SetSourceAsync(stream);
            }
        }

        private async void setPanel()
        {
            this.panel = new BitmapImage();
            using(var stream = await this.panelFile.OpenReadAsync())
            {
                await this.panel.SetSourceAsync(stream);
            }
        }

        public async void UpdateSeriesData()
        {
            this.watchData = await this.getFolder().CreateFileAsync("view.dat", CreationCollisionOption.ReplaceExisting);
            this.episodeData = await WriteData(this.watchData, this.currentEpisode, this.watchState); 
        }

        public async Task GetSeriesData()
        {
            StorageFile data = this.getData();
            EpisodeData ep = null;
            SeriesState state;
            if(data != null)
            {
                ep = await ReadData(data);
                state = (SeriesState)ep.watchStatus;
            } else
            {
                data = await this.getFolder().CreateFileAsync("view.dat", CreationCollisionOption.ReplaceExisting);
                state = SeriesState.Incomplete;
                ep = await WriteData(data, 0, state);
            }
            switch(state)
            {
                case SeriesState.Incomplete:
                    break;
                case SeriesState.Watching:
                    break;
                case SeriesState.Complete:
                    break;
                case SeriesState.Dropped:
                    break;
            }
            this.episodeData = ep;
            this.setWatchState(state);
            this.currentEpisode = ep.currentEpisode;
        }

        private async Task<EpisodeData> ReadData(StorageFile d)
        {
            EpisodeData episodeData = null;
            string data = await FileIO.ReadTextAsync(d);
            int watched = Convert.ToInt32(Char.GetNumericValue(data[0]));
            SeriesState state = (SeriesState)Convert.ToInt32(Char.GetNumericValue(data[2]));
            episodeData = new EpisodeData(watched, state, d);
            return episodeData;

        }

        private async Task<EpisodeData> WriteData(StorageFile f, int watched, SeriesState state)
        {
            await FileIO.WriteTextAsync(f, watched + "\n" + ((int)state).ToString());
            return new EpisodeData(watched, state, f);
        }
    }
}
