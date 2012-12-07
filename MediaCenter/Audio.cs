using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class Audio : Media
    {
        public enum AudioType{ wav, mp3, aac, wma, m4a, ogg, flac }

        private AudioType _audioType;

        public Audio() { }

        public Audio(String name, String path, long size, Int32 rating, AudioType audioType)
            : base(name, path, size, rating)
        {
            this._audioType = audioType;
        }

        public void SetAudioType(AudioType audioType)
        {
            this._audioType = audioType;
        }

        public AudioType GetAudioType()
        {
            return this._audioType;
        }
    }
}
