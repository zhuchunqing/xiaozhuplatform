using BisPlatformWeb.Common;
using BisPlatformWeb.Hangfire.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisPlatformWeb.Hangfire.Implementation
{
    public class Myjob:IMyjob
    {
        public void RunAtTimeOf(DateTime now)
        {
            NlogHelper.InfoLog("1234123");
        }
    }
}
