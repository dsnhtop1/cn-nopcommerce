using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.GoogleAnalytics
{
    /// <summary>
    /// Google Analytic plugin
    /// </summary>
    public class GoogleAnalyticPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly GoogleAnalyticsSettings _googleAnalyticsSettings;


        public GoogleAnalyticPlugin(IWebHelper webHelper, ISettingService settingService, 
            GoogleAnalyticsSettings googleAnalyticsSettings)
        {
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._googleAnalyticsSettings = googleAnalyticsSettings;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return !string.IsNullOrWhiteSpace(_googleAnalyticsSettings.WidgetZone)
                       ? new List<string>() { _googleAnalyticsSettings.WidgetZone }
                       : new List<string>() { "body_end_html_tag_before" };
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsGoogleAnalytics/Configure";
        }

        /// <summary>
        /// Gets a name of a view component for displaying widget
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <returns>View component name</returns>
        public string GetWidgetViewComponentName(string widgetZone)
        {
            return "WidgetsGoogleAnalytics";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            var settings = new GoogleAnalyticsSettings
            {
                GoogleId = "UA-0000000-0",
                TrackingScript = @"<!-- Google code for Analytics tracking -->
                    <script type=""text/javascript"">
                    var _gaq = _gaq || [];
                    _gaq.push(['_setAccount', '{GOOGLEID}']);
                    _gaq.push(['_trackPageview']);
                    {ECOMMERCE}
                    (function() {
                        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
                        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
                    })();
                    </script>",
            };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId", "ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint", "Enter Google Analytics ID.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript", "Tracking code with {ECOMMERCE} line");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint", "Paste the tracking code generated by Google Analytics here. {GOOGLEID} and will be dynamically replaced.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EnableEcommerce", "Enable E-commerce");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EnableEcommerce.Hint", "Check to pass information about orders to Google E-commerce feature.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax", "Include tax");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax.Hint", "Check to include tax when generating tracking code for {ECOMMERCE} part.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Instructions", "<p>Google Analytics is a free website stats tool from Google. It keeps track of statistics about the visitors and eCommerce conversion on your website.<br /><br />Follow the next steps to enable Google Analytics integration:<br /><ul><li><a href=\"http://www.google.com/analytics/\" target=\"_blank\">Create a Google Analytics account</a> and follow the wizard to add your website</li><li>Copy the Tracking ID into the 'ID' box below</li><li>Click the 'Save' button below and Google Analytics will be integrated into your store</li></ul><br />If you would like to switch between Google Analytics (used by default) and Universal Analytics, then please use the buttons below:</p>");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Note", "<p><em>Please note that {ECOMMERCE} line works only when you have \"Disable order completed page\" order setting unticked.</em></p>");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<GoogleAnalyticsSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.GoogleId.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.TrackingScript.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EnableEcommerce");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.EnableEcommerce.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.IncludingTax.Hint");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Instructions");
            this.DeletePluginLocaleResource("Plugins.Widgets.GoogleAnalytics.Note");

            base.Uninstall();
        }
    }
}