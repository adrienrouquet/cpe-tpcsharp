using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class Audio : Media
    {
        private String _audioType;

        public Audio() { }

        public Audio(String name, String path, String size, Int32 rating, String audioType)
            : base(name, path, size, rating)
        {
            this._audioType = audioType;
        }

        public void SetAudioType(String audioType)
        {
            this._audioType = audioType;
        }

        public String GetAudioType()
        {
            return this._audioType;
        }
    }
}
