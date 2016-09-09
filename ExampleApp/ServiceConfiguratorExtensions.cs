using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf.ServiceConfigurators;

namespace ExampleApp
{
    public static class ServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<SIPService> ConstructUsing(this ServiceConfigurator<SIPService> self, Action<SIPServiceConfigurator> configure)
        {
            var configurator = new SIPServiceConfigurator();
            configure(configurator);
            self.ConstructUsing((a) => configurator.Build());
            return self;
        }

    }
}
