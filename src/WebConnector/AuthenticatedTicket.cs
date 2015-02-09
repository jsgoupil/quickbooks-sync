using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.WebConnector
{
    public class AuthenticatedTicket
    {
        public virtual string Ticket { get; set; }
        public virtual int CurrentStep { get; set; }
        public virtual bool Authenticated { get; set; }
    }
}
