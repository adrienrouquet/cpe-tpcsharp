using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter
{
    public class Media
    {
        private static Int32 _nextID = 0;
        private Int32 _ID = ++_nextID;
        private String _name;
        private String _path;
        private long _size;
        private Int32 _rating;

        public Media()
        {
        }

        public Media(String name, String path, long size, Int32 rating)
        {
            this._name = name;
            this._path = path;
            this._size = size;
            this._rating = rating;
        }

        public void SetID(Int32 ID)
        {
            this._ID = ID;
        }
        public Int32 GetID()
        {
            return this._ID;
        }
        
        public void SetName(String name)
        {
            this._name = name;
        }
        public String GetName()
        {
            return this._name;
        }

        public void SetPath(String path)
        {
            this._path = path;
        }
        public String GetPath()
        {
            return this._path;
        }

        public void SetSize(long size)
        {
            this._size = size;
        }
        public long GetSize()
        {
            return this._size;
        }

        public void SetRating(Int32 rating)
        {
            this._rating = rating;
        }
        public Int32 GetRating()
        {
            return this._rating;
        }
    }
}
