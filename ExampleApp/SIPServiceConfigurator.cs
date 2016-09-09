using Castle.Windsor;
using pjsip4net.Configuration;
using pjsip4net.Container.Castle;
using pjsip4net.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp
{
    public class SIPServiceConfigurator
    {
        private Configure cfg;

        public SIPServiceConfigurator()
        {
            //cfg = Configure.Pjsip4Net();//.FromConfig();
            //Configure.Pjsip4Net().With(x => x.Config.AutoAnswer = false).With(x => x.Config.OutboundProxies.Add("sip:192.168.1.1:5060")).Build().Start();

            var container = new WindsorContainer();

            cfg = Configure.Pjsip4Net()


                .With_CastleContainer(container)//plugs an existing DI-container





                .With(x => x.LoggingConfig.TraceAndDebug = true)
                .With(x => x.LoggingConfig.LogMessages = true)
                .With(x => x.LoggingConfig.LogLevel = 100)
                //.With(x=>x.MediaConfig.)
                .With(x => x.Config.AutoAnswer = false)
                .With(x => x.Config.AutoConference = false)
                .With(x => x.Config.AutoRecord = false)
                //.With(x => x.Config.Credentials = false)
                //.With(x => x.Config.DnsServers = false)
                //.With(x => x.Config.ForceLooseRoute = false)
                //.With(x => x.Config.HangupForkedCall = false)
                //.With(x => x.Config.SecureSignalling = false)
                //.With(x => x.Config.ThreadCount = 1)
                .With(x => x.Config.UseSrtp = pjsip4net.Core.Data.SrtpRequirement.Disabled);

        }


        public SIPService Build()
        {
            return new SIPService(cfg);
        }


        public SIPServiceConfigurator UseConfig()
        {
            cfg = cfg.FromConfig();
            return this;
        }

        public SIPServiceConfigurator UseTcp()
        {
            cfg = cfg.With(o =>
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Bind(new IPEndPoint(IPAddress.Any, 0)); // Pass 0 here.
                var tcpPort = ((IPEndPoint) sock.LocalEndPoint).Port;
                sock.Dispose();

                var tcpTransportType = new pjsip4net.Core.Data.TransportConfig()
                {
                    BoundAddress = "0.0.0.0",
                    Port = (uint) tcpPort,
                    PublicAddress = "0.0.0.0",
                };

                var tcp = new pjsip4net.Core.Utils.Tuple<pjsip4net.Core.TransportType, pjsip4net.Core.Data.TransportConfig>
                    (pjsip4net.Core.TransportType.Tcp, tcpTransportType);
                o.RegisterTransport(tcp);
            });

            return this;
        }


        public SIPServiceConfigurator UseUdp()
        {
            cfg = cfg.With(o =>
            {
                //Socket socku = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //socku.Bind(new IPEndPoint(IPAddress.Any, 0)); // Pass 0 here.
                //var udpPort = ((IPEndPoint) socku.LocalEndPoint).Port;
                //socku.Dispose();


                var udpTransportType = new pjsip4net.Core.Data.TransportConfig()
                {
                    BoundAddress = "0.0.0.0",
                    //Port = 0,
                    PublicAddress = "0.0.0.0",
                };

                var udp = new pjsip4net.Core.Utils.Tuple<pjsip4net.Core.TransportType, pjsip4net.Core.Data.TransportConfig>
                        (pjsip4net.Core.TransportType.Udp, udpTransportType);
                o.RegisterTransport(udp);
            });

            return this;
        }

        public SIPServiceConfigurator WithTcpSipTransport()
        {
            cfg = cfg.WithSipTransport(o=>
            {
                return new pjsip4net.Core.Utils.Tuple<pjsip4net.Core.TransportType, pjsip4net.Core.Data.TransportConfig>(pjsip4net.Core.TransportType.Tcp, new pjsip4net.Core.Data.TransportConfig()
                {
                    // BoundAddress=,
                    //  Port=,
                    //   PublicAddress=,
                    //    TlsSetting=new pjsip4net.Core.Data.TlsConfig()
                    //    {
                    //         CAListFile 
                    //    }
                });
            });

            return this;
        }
        public SIPServiceConfigurator WithUdpSipTransport()
        {
            cfg = cfg.WithSipTransport(o =>
            {
                return new pjsip4net.Core.Utils.Tuple<pjsip4net.Core.TransportType, pjsip4net.Core.Data.TransportConfig>
                (pjsip4net.Core.TransportType.Udp, new pjsip4net.Core.Data.TransportConfig()
                {
                    // BoundAddress=,
                    //  Port=,
                    //   PublicAddress=,
                    //    TlsSetting=new pjsip4net.Core.Data.TlsConfig()
                    //    {
                    //         CAListFile 
                    //    }
                });
            });

            return this;
        }


        public SIPServiceConfigurator WithDefaultAccount()
        {
            cfg = cfg.WithAccounts(o =>
            {
      ////< account accountId = "sip:1000@66.240.xxx.xx:5080" registrarUri = "sip:66.240.xx.xx:5080"
      ////    userName = "1000" password = "1234" realm = "*" isDefault = "true"
      ////    publishPresence = "false" />

                var x = new List<pjsip4net.Core.Data.AccountConfig>();
                x.Add(new pjsip4net.Core.Data.AccountConfig()
                {
                    Id = @"sip:1000@66.240.xxx.xx:5080",
                    RegUri = @"sip:66.240.xx.xx:5080",
                     IsDefault=true,
                      //realm
                      //username
                      //password
                       IsPublishEnabled = false,
                });


                return x;
                


                });

            return this;
        }





        public SIPServiceConfigurator AddOutboundProxy(Uri sipUri)
        {
            cfg = cfg.With(o =>
            {
                o.Config.OutboundProxies.Add(sipUri.ToString());
            });

            return this;
        }



    }
}
