using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaCenter
{
    class Image : Media
    {
        public Image(String name, String path, String size, Int32 rating) : base(name, path, size, rating)
        {
        }
    }
}
