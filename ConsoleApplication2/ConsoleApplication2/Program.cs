using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelldusWrapper;

// se https://github.com/telldus/telldus/blob/master/bindings/dotnet/example/BasicListDevicesNetExample.zip

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            var antal = TelldusNETWrapper.tdGetNumberOfDevices();
            for (var i = 1; i < antal; i++)
            {
            }
        }
    }
}
