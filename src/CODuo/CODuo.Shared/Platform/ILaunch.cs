using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CODuo.Platform
{
    public interface ILaunch
    {
        Task EMail(string to);

        Task Web(string address);

        Task Twitter(string at);

        Task Phone(string number);
    }
}
