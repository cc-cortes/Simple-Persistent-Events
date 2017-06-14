
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Activation;
using Microsoft.Toolkit.Uwp;

namespace TestApp
{
    public static class BackgroundActivity
    {
        public static void TimeZoneChange(object sender, PersistentEventArgs e)
        {
            var def = e.GetDeferral();
            DisplayToast("Time Zone Change Persistent event");
            def.Complete();
        }

        public static void UserPresent(object sender, PersistentEventArgs e)
        {
            var def = e.GetDeferral();
            DisplayToast("User Present Persistent event");
            def.Complete();
        }

        public static void UserAway(object sender, PersistentEventArgs e)
        {
            var def = e.GetDeferral();
            DisplayToast("User Away Persistent event");
            def.Complete();
        }

        public static void MaintenanceWindow(object sender, PersistentEventArgs e)
        {
            var def = e.GetDeferral();
            DisplayToast("Maintenance Window Persistent event");
            def.Complete();
        }

        private static ToastNotification DisplayToast(string content)
        {
            string xml = $@"<toast activationType='foreground'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                    <text>Persistent Events App</text>
                                                </binding>
                                            </visual>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            var el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el); //Add content to notification

            var toast = new ToastNotification(doc);

            ToastNotificationManager.CreateToastNotifier().Show(toast); //Show the toast

            return toast;
        }
    }

}