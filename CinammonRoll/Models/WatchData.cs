using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CinammonRoll.Models
{
    public class WatchData
    {
        private List<Series> incompleteList;
        private List<Series> watchList;
        private List<Series> completedList;
        private List<Series> droppedList;

        public WatchData()
        {
            this.watchList = new List<Series>();
            this.droppedList = new List<Series>();
            this.completedList = new List<Series>();
            this.incompleteList = new List<Series>();
        }

        public void AddSeries(Series s)
        {
            SeriesState state = s.watchState;
            switch(state)
            {
                case SeriesState.Incomplete:
                    this.incompleteList.Add(s);
                    break;
                case SeriesState.Watching:
                    this.watchList.Add(s);
                    break;
                case SeriesState.Complete:
                    this.completedList.Add(s);
                    break;
                case SeriesState.Dropped:
                    this.droppedList.Add(s);
                    break;
            }
        }

        public List<Series> GetSeriesList(List<Series> list, SeriesState targetState)
        {
            List<Series> result = new List<Series>();
            foreach (Series s in list)
            {
                SeriesState state = s.watchState;
                if (state == targetState)
                {
                    result.Add(s);
                }
            }

            return result;
        }
    }

    public class EpisodeData
    {
        public int currentEpisode;
        public int watchStatus;
        private StorageFile file;

        public EpisodeData(int currentEpisode, SeriesState state, StorageFile file)
        {
            this.currentEpisode = currentEpisode;
            this.watchStatus = (int)state;
            this.file = file;
        }

        public StorageFile GetFile()
        {
            return this.file;
        }
    }

    public enum SeriesState
    {
        Incomplete = 0,
        Watching = 1,
        Complete = 2,
        Dropped = 3
    }
}
