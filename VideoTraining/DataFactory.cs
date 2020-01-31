using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace UCExtend.VideoTraining
{
    public static class DataFactory
    {
        //Video settings
        public static string settingsFolderBase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + Application.CompanyName + @"\" + Application.ProductName;
        public static string pathVideoPlaylist = settingsFolderBase + @"\VideoPlayList.txt";
        public static string pathVideoPlayWatchList = settingsFolderBase + @"\VideoWatchList.txt";

        public static List<Video> LoadData()
        {
            List<Video> videos = new List<Video>();

            var playList = GetPlayList();
            var playWatchList = GetPlayWatchList();

            foreach (var item in playList)
            {
                var video = new Video
                {
                    Url = item,
                    IsWatched = playWatchList.Contains(item)
                };
                videos.Add(video);
            }

            return videos;
        }

        public static List<string> GetPlayList()
        {
            List<string> videoPlaylist = new List<string>();

            if (File.Exists(pathVideoPlaylist))
            {
                var lines = System.IO.File.ReadAllLines(pathVideoPlaylist);
                foreach (var line in lines)
                {
                    var split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    videoPlaylist.Add(line);
                }
            }
            else
            {
                //MessageBox.Show("No video playlist found!");
            }

            return videoPlaylist;

        }

        public static List<string> GetPlayWatchList()
        {
            List<string> videoWatchlist = new List<string>();

            if (File.Exists(pathVideoPlayWatchList))
            {
                var lines = System.IO.File.ReadAllLines(pathVideoPlayWatchList);
                foreach (var line in lines)
                {
                    var split = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    videoWatchlist.Add(line);
                }
            }
            else
            {
                File.Create(pathVideoPlayWatchList);
            }

            return videoWatchlist;
        }

    }
}
