using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class MediaList
    {
        private List<Media> _mediaList = new List<Media>();

        public MediaList()
        {

        }

        public void SetMediaList (List<Media> mediaList)
        {
            this._mediaList = mediaList;
        }
        public List<Media> GeMediaList()
        {
            return this._mediaList;
        }

        public void AddMedia(Media media)
        {
            this._mediaList.Add(media);
        }

        public Media GetMedia(Int32 id)
        {
            return this._mediaList.Find(Media => Media.GetID() == id);
        }
    }
}
