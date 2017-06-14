# Persistent Events

**Persistent Events** are a way to register for background activations using an easy event subscription pattern. 
Subscribing to an event will enable the app to wake up if it is Not Running in order to handle the event. In order to
stop getting the app woken up unsubscribe all handlers you haver for the event. This library wraps the background task
registration methods and is available on 15063 and above.

Persistent events are particularly useful for middleware developers writing SDKs or tools for UWP apps. A Persistent Event
can be used to provide an interface for app developers to wake up their app. For example, an instant messaging or VoIP SDK could create
a Persistent Event for an incoming message that is delivered via the PushNotificationTrigger or SocketActivityTrigger. A
hardware developer could provide a Persistent Event that notifies apps when their device has been connected. 

## Example

Persistent Events are events. They can be subscribed to in the Application constructor like other App events such
as Suspending or Resuming. Handle the event by taking a deferral and subscribing to the canceled event. These will
enable the app to run in the background using asynchronous methods for around twenty five seconds, with a five second
time period to handle the canceled event.

```csharp
	
    // Be sure to include the using at the top of the file:
    //using Microsoft.Toolkit.Uwp;

    // Subscribe to an event using an event handler in the App() constructor
    PersistentEvents.UserPresent += BackgroundActivity.UserPresent;

    //Unregister if there is a point where you no longer want to be activated for an event
	PersistentEvents.UserPresent += BackgroundActivity.UserPresent;

    // The event handler should account for the background activated event args and hold the deferral while doing work
    private void UserPresent(object sender, PersistentEventArgs e)
    {
	    //Request a deferral to do asynchronous work while in the background
        var def = e.GetDeferral();

		//Subscribe for the possible cancelation of the event
		e.Canceled += EventCanceled;

        //Do work here
        def.Complete();
    }

	private void EventCanceled(object sender, PersistentEventCanceledArgs args)
	{
		//Clean up here in case of cancellation
	}
```

## Events Available

* **UserPresent** indicates the user has started interacting with the device.
* **UserAway** indicates that the user has recently stopped interacting with the device.
* **MaintenanceWindow** indicates that the device is on AC power and has entered a time window where apps can perform background syncing.
* **TimeZoneChange** indicates the time zone of the device has changed.

### Resources

You can find more examples of background activity in the [UWP Documentation](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/).
You can also find more helpers for background tasks in the [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit/blob/dev/Microsoft.Toolkit.Uwp/Helpers/BackgroundTaskHelper.cs).

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, 10.0.15063.0 or higher |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp |

## API

* [Persistent Events source code](https://github.com/Microsoft/UWPCommunityToolkit/blob/master/Microsoft.Toolkit.Uwp/Helpers/PersistentEvents)
