using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSM.Classes
{
    public class Picture
    {
        private Decimal _picID;

        public Decimal PicID
        {
            get { return _picID; }
            set { _picID = value; }
        }
        private Decimal _albumID;

        public Decimal AlbumID
        {
            get { return _albumID; }
            set { _albumID = value; }
        }
        private String _picPath;

        public String PicPath
        {
            get { return _picPath; }
            set { _picPath = value; }
        }
        private String _picName;

        public String PicName
        {
            get { return _picName; }
            set { _picName = value; }
        }
        private String _picDesc;

        public String PicDesc
        {
            get { return _picDesc; }
            set { _picDesc = value; }
        }
        private DateTime _picDate;

        public DateTime PicDate
        {
            get { return _picDate; }
            set { _picDate = value; }
        }

        /// <summary>
        /// Method to return a random properties for display
        /// </summary>
        /// <returns>[0] = top, [1] = left, [2] = rotate</returns>
        public int[] GetProperties()
        {
            Random r = new Random();
            Thread.Sleep(50);
            int rotate = r.Next(0, 80) - 40;
            int top = r.Next(0, 500);
            int left = r.Next(0, 400);

            if (top > 270 && left > 270)
            {
                top -= 120 + 130;
                left -= 230;
            }

            return new int[3] { top, left, rotate };

        }
    }
}
