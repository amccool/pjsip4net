using pjsip4net.Configuration;
using pjsip4net.Core.Configuration;
using pjsip4net.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleApp
{
    public class SIPService
    {
        private readonly ISipUserAgent ua;

        public SIPService(Configure cfg)
        {
            ua = cfg.Build();
        }


        public bool Start()
        {

            ua.Log += Ua_Log;
            ua.CallManager.CallRedirected += CallManager_CallRedirected;
            ua.CallManager.CallStateChanged += CallManager_CallStateChanged;
            ua.CallManager.CallTransfer += CallManager_CallTransfer;
            ua.CallManager.IncomingCall += CallManager_IncomingCall;
            ua.CallManager.IncomingDtmfDigit += CallManager_IncomingDtmfDigit;
            ua.CallManager.Ring += CallManager_Ring;

            ua.AccountManager.AccountStateChanged += AccountManager_AccountStateChanged;

            ua.Start();

            var da = ua.AccountManager.DefaultAccount;

            IAccount account = ua.AccountManager.Register(o =>
            {
                //return o.At("doink").WithExtension("alex.mccool").WithPassword("password").Register();
                //return o.At("officesip.local").WithExtension("rb-test").WithPassword(string.Empty).Register();
                return o.At("doink").WithExtension("phoneservice").WithPassword(string.Empty).Register();
            });


            var call = ua.CallManager.MakeCall(
                x => x.To("998101").At("devint.dev-r5ead.net").From(account)
            //.Through(_args.Through)
            //.From(_agent.AccountManager.GetAccountById(Convert.ToInt32(_args.From)))
            .Call());



            return true;
        }

        private void AccountManager_AccountStateChanged(object sender, pjsip4net.Accounts.AccountStateChangedEventArgs e)
        {
            Trace.WriteLine(e.StatusText);
        }

        private void CallManager_Ring(object sender, pjsip4net.Calls.RingEventArgs e)
        {
            Trace.WriteLine(e.CallId);
        }

        private void CallManager_IncomingDtmfDigit(object sender, pjsip4net.Calls.DtmfEventArgs e)
        {
            Trace.WriteLine("{e.CallId} {e.Digit}");
        }

        private void CallManager_IncomingCall(object sender, pjsip4net.Core.Utils.EventArgs<ICall> e)
        {
            Trace.WriteLine("{e.Data}");
        }

        private void CallManager_CallTransfer(object sender, pjsip4net.Calls.CallTransferEventArgs e)
        {
            Trace.WriteLine(e.Destination);
        }

        private void CallManager_CallStateChanged(object sender, pjsip4net.Calls.CallStateChangedEventArgs e)
        {
            Trace.WriteLine(e.InviteState);
        }

        private void CallManager_CallRedirected(object sender, pjsip4net.Calls.CallRedirectedEventArgs e)
        {
            Trace.WriteLine(e.Target);
        }

        private void Ua_Log(object sender, pjsip4net.LogEventArgs e)
        {
            Trace.WriteLine(e.Data);
        }

        public bool Stop()
        {
            if (ua != null)
            { ua.Destroy(); }

            if (ua != null)
            { ua.Dispose(); }

            return true;
        }

    }
}
