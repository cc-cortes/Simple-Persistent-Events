using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Background;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace Microsoft.Toolkit.Uwp
{
    //System Persistent Events

    public static partial class PersistentEvents
    {

        /// <summary>
        /// Subscribing to this event to activate the app in the background when the user begins interacting with the device
        /// </summary>
        /// <remarks>
        /// Subscribe to this event in the Application constructor or Activated callback.
        /// </remarks>
        public static event EventHandler<PersistentEventArgs> UserPresent
        {
            add
            {
                if (UserPresentEvent == null)
                {
                    UserPresentEvent = new PersistentEvent("User Present Persistent Event", new SystemTrigger(SystemTriggerType.UserPresent, false));
                }
                return UserPresentEvent.Add(value);
            }
            remove { UserPresentEvent.Remove(value); }
        }

        private static PersistentEvent UserPresentEvent;

        /// <summary>
        /// Subscribing to this event to activate the app in the background when the user stops interacting with the device and the device goes into idle.
        /// </summary>
        /// <remarks>
        /// Subscribe to this event in the Application constructor or Activated callback.
        /// </remarks>
        public static event EventHandler<PersistentEventArgs> UserAway
        {
            add
            {
                if (UserAwayEvent == null)
                {
                    UserAwayEvent = new PersistentEvent("User Away Persistent Event", new SystemTrigger(SystemTriggerType.UserAway, false));
                }
                return UserAwayEvent.Add(value);
            }
            remove { UserAwayEvent.Remove(value); }
        }

        private static PersistentEvent UserAwayEvent;

        /// <summary>
        /// Subscribing to this event to activate the app in the background when the device enters a maintenance window for supported background app syncing.
        /// </summary>
        /// <remarks>
        /// Subscribe to this event in the Application constructor or Activated callback.
        /// </remarks>
        public static event EventHandler<PersistentEventArgs> MaintenanceWindow
        {
            add
            {
                if (MaintenanceWindowEvent == null)
                {
                    MaintenanceWindowEvent = new PersistentEvent("Maintenance Window Persistent Event", new MaintenanceTrigger(60, false));
                }
                return MaintenanceWindowEvent.Add(value);
            }
            remove { MaintenanceWindowEvent.Remove(value); }
        }

        private static PersistentEvent MaintenanceWindowEvent;

        /// <summary>
        /// Subscribing to this event to activate the app in the background when the Time Zone Changes on the device.
        /// </summary>
        /// <remarks>
        /// Subscribe to this event in the Application constructor or Activated callback.
        /// </remarks>
        public static event EventHandler<PersistentEventArgs> TimeZoneChange
        {
            add
            {
                if (TimeZoneChangeEvent == null)
                {
                    TimeZoneChangeEvent = new PersistentEvent("Time Zone Change Persistent Event", new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
                }
                return TimeZoneChangeEvent.Add(value);
            }
            remove { TimeZoneChangeEvent.Remove(value); }
        }

        private static PersistentEvent TimeZoneChangeEvent;

    }
}
