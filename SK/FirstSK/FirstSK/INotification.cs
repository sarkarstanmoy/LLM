using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstSK
{
    internal interface INotification
    {
        public void SendNotification();

        public void ConfigureNotifiation();
    }
}