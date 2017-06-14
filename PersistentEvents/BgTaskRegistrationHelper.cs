using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Microsoft.Toolkit.Uwp
{
    static class BgTaskRegistrationHelper
    {
        private const String TaskGroupName = "UWP Community Toolkit Persistent Events";
        private const String TaskGroupId = "3F2504E0-5F89-41E3-9A1C-0405E82C3331";

        /// <summary>
        /// Register a background task with the specified name, trigger, and condition (optional).
        /// </summary>
        /// <param name="name">A name for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">An optional conditional event that must be true for the task to fire.</param>
        public static BackgroundTaskRegistration RegisterBackgroundTask(String name, IBackgroundTrigger trigger, IBackgroundCondition condition = null)
        {
            BackgroundTaskRegistration task = GetTaskRegistration(name);

            if(task != null)
            {
                return task;
            }

            //Request access to run in the background
            var requestTask = BackgroundExecutionManager.RequestAccessAsync();

            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.SetTrigger(trigger);
            builder.IsNetworkRequested = true;

            if (condition != null)
            {
                builder.AddCondition(condition);

                //
                // If the condition changes while the background task is executing then it will
                // be canceled.
                //
                builder.CancelOnConditionLoss = true;
            }

            var group = GetTaskGroup(TaskGroupId, TaskGroupName);
            builder.TaskGroup = group;

            task = builder.Register();

            return task;
        }

        /// <summary>
        /// Unregister background tasks with specified name.
        /// </summary>
        /// <param name="name">Name of the background task to unregister.</param>
        public static BackgroundTaskRegistration UnregisterBackgroundTasks(String name)
        {
            BackgroundTaskRegistration registration = null;

            // Loop through all background tasks associated with the task group and unregister any with the given name.
            var group = GetTaskGroup(TaskGroupId, TaskGroupName);

            foreach (var cur in group.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                    registration = cur.Value;
                }
            }

            return registration;
        }

        /// <summary>
        /// Return a background task registration if it has already been registered with the specified name.
        /// </summary>
        /// <param name="name">The background task name</param>
        /// <returns></returns>
        private static BackgroundTaskRegistration GetTaskRegistration(String name)
        {
            var group = GetTaskGroup(TaskGroupId, TaskGroupName);

            foreach (var cur in group.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    return cur.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieve a registered background task group. If no group is registered with the given id,
        /// then create a new one and return it.
        /// </summary>
        /// <returns>The task group associated with the given id</returns>
        private static BackgroundTaskRegistrationGroup GetTaskGroup(string id, string groupName)
        {
            var group = BackgroundTaskRegistration.GetTaskGroup(id);

            if (group == null)
            {
                group = new BackgroundTaskRegistrationGroup(id, groupName);
            }

            return group;
        }

    }
}
