using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace CSM.Classes
{
    public interface ITicket : IIdentity
    {

        DateTime LoginDate
        {
            get;
        }
        
        String SessionID
        {
            get;
        }
    }
}
