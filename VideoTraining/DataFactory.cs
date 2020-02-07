using LiteDB;
using LiteDB.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public static string liteDBPath = settingsFolderBase + @"\VideoPlayList.db";

        public static List<Video> GetVideos()
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

        //LiteDB
        //https://www.c-sharpcorner.com/UploadFile/ranjancse/getting-started-with-litedb/
        public static void Add(Video issue)
        {
            // Open data file (or create if not exits)  
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issueCollection = db.GetCollection<Video>("issues");
                // Insert a new issue document  
                issueCollection.Insert(issue);
                //IndexIssue(issueCollection);
            }            
        }



        private static void IndexIssue(Collection<Video> issueCollection)
        {
            //// Index on IssueId  
            //issueCollection.EnsureIndex(x => x.IssueId);
            //// Index on ErrorText  
            //issueCollection.EnsureIndex(x => x.ErrorText);
            //// Index on DateTime  
            //issueCollection.EnsureIndex(x => x.DateTime);
            //// Index on IssueType  
            //issueCollection.EnsureIndex(x => x.IssueType);
        }

        public static void Update(Video issue)
        {
            // Open data file (or create if not exits)  
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issueCollection = db.GetCollection<Video>("issues");
                // Update an existing issue document  
                issueCollection.Update(issue);
            }
        }

        public static void Delete(Guid issueId)
        {
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issues = db.GetCollection<Video>("issues");
                //issues.Delete(i => i.IssueId == issueId);
            }
        }

        public static IList<Video> GetAll()
        {
            var issuesToReturn = new List<Video>();
            using (var db = new LiteDatabase(liteDBPath))
            {
                var issues = db.GetCollection<Video>("issues");
                //var results = issues.All();
                var results = issues.FindAll();
                foreach (Video issueItem in results)
                {
                    issuesToReturn.Add(issueItem);
                }
                return issuesToReturn;
            }
        }

    }
}
