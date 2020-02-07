using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCExtend.VideoTraining
{
    public class Video
    {
        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string videoId;
        public string VideoId
        {
            get { return videoId; }
            set { videoId = value; }
        }

        private bool isWatched;
        public bool IsWatched
        {
            get { return isWatched; }
            set { isWatched = value; }
        }

        private int offerCount;
        public int OfferCount
        {
            get { return offerCount; }
            set { offerCount = value; }
        }
    }
}
