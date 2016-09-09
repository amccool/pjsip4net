using pjsip4net.Core.Configuration;

namespace pjsip.Interop
{
    /// <summary>
    /// This configurator registers services that enable access to underlying pjsip 1.4 library pjsua API.
    /// </summary>
    public static class ConfigureVersion_2_5
    {
        /// <summary>
        /// Configure with pjsip version 1.4.
        /// </summary>
        public static Configure WithVersion_2_5(this Configure cfg)
        {
            return cfg.With(new Version_2_5_ComponentConfigurator());
        }
    }
}