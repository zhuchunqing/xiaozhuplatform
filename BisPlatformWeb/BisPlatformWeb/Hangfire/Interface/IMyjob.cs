using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisPlatformWeb.Hangfire.Interface
{
    public interface IMyjob
    {
        void RunAtTimeOf(DateTime now);
    }
}
