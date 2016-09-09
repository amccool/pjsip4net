using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<SIPService>(s =>
                {
                    s.ConstructUsing(sc =>
                    {
                        //sc.UseConfig();
                        sc.WithUdpSipTransport();
                        //sc.WithTcpSipTransport();
                        //sc.UseUdp();  // use only tcp OR udp , not both
                    });


                    s.WhenStarted(service => service.Start());

                    s.WhenStopped(service => service.Stop());
                });


                x.RunAsNetworkService();
            });

        }
    }
}
