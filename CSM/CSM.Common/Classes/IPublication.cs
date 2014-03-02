using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSM.Classes
{
    public interface IPublication
    {
        User User
        {
            get;
        }

        Decimal PublID
        {
            get;
        
        }

        Picture Image
        {
            get;
            set;
        }
    }
}
