
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace Microsoft.Toolkit.Uwp
{
    internal class PersistentEvent
    {
        private String mTaskName;
        private IBackgroundTrigger mTrigger;
        private IBackgroundCondition mCondition;
        private EventRegistrationTokenTable<EventHandler<PersistentEventArgs>> mTokenTable = null;
        private bool? mIsGroupApiPresent;


        public PersistentEvent(string name, IBackgroundTrigger trigger, IBackgroundCondition condition = null)
        {
            // Set the 3 members required to register a background task
            mTaskName = name;
            mTrigger = trigger;
            mCondition = condition;
        }

        public EventRegistrationToken Add(EventHandler<PersistentEventArgs> value)
        {
            // If the background task registration group API is not supported on this build then return without registering
            if (!IsGroupApiPresent()) { return new EventRegistrationToken(); }

            // Register the background task
            var reg = BgTaskRegistrationHelper.RegisterBackgroundTask(mTaskName, mTrigger, mCondition);

            // Get the event token table
            var table = EventRegistrationTokenTable<EventHandler<PersistentEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref mTokenTable);

            // If this is the first subscription to this Persistent Event then subscribe to the task group's background activated event
            if (table == null || table.InvocationList == null)
            {
                reg.TaskGroup.BackgroundActivated += TaskGroup_BackgroundActivated;
            }

            // Add the event handler to the table and return the registration token
            return table.AddEventHandler(value);
        }

        public void Remove(EventRegistrationToken value)
        {
            // If the background task registration group API is not supported on this build then return without doing work
            if (!IsGroupApiPresent()) { return; }

            // Get the event token table and remove the event handler
            var table = EventRegistrationTokenTable<EventHandler<PersistentEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref mTokenTable);
            table.RemoveEventHandler(value);

            // If there are no more event handlers for this Persistent Event then unregister the background task and unsubscribe from the group's background activated event
            if (table == null || table.InvocationList == null)
            {
                var reg = BgTaskRegistrationHelper.UnregisterBackgroundTasks(mTaskName);
                reg.TaskGroup.BackgroundActivated -= TaskGroup_BackgroundActivated;
            }
        }

        private void TaskGroup_BackgroundActivated(BackgroundTaskRegistrationGroup sender, BackgroundActivatedEventArgs args)
        {
            // Check to confirm the correct task is what caused the activation
            if (args.TaskInstance.Task.Name == mTaskName)
            {
                // Get the registration token table and get the delegate to invoke all event handlers that have been subscribed
                var table = EventRegistrationTokenTable<EventHandler<PersistentEventArgs>>.GetOrCreateEventRegistrationTokenTable(ref mTokenTable);
                table.InvocationList?.Invoke(null, new PersistentEventArgs(args));
            }
        }
        
        private bool IsGroupApiPresent()
        {
            // Save a cached version of the API Information check
            if(mIsGroupApiPresent == null)
            {
                mIsGroupApiPresent = Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.BackgroundTaskRegistrationGroup");
            }

            return (bool)mIsGroupApiPresent;
        }
    }
}