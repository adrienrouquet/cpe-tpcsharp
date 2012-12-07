using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class Video : Media
    {
        private Boolean _HD = false;

        public Video() { }

        public Video(String name, String path, String size, Int32 rating, Boolean HD) : base(name, path, size, rating)
        {
            this._HD = HD;
        }
        
        public void ChangeQuality()
        {
            this._HD = !(this._HD);
        }
        public Boolean IsHD()
        {
            return this._HD;
        }
    }
}
