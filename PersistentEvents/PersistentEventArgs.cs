using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp
{
    public sealed class PersistentEventArgs
    {
        public event PersistentEventCancelationHandler Canceled;
        private BackgroundActivatedEventArgs m_backgroundActivatedArgs;
        private BackgroundTaskDeferral m_def;
        private int m_defCount = 0;
        private PersistentEventArgs m_selfRef;

        public Deferral GetDeferral()
        {
            m_defCount++;

            if(m_def == null)
            {
                m_def = m_backgroundActivatedArgs.TaskInstance.GetDeferral();
            }

            //Hold this ref until all deferrals are complete
            m_selfRef = this;

            return new Deferral(new DeferralCompletedHandler(CompleteDeferral));
        }

        public PersistentEventArgs(BackgroundActivatedEventArgs backgroundActivatedEventArgs)
        {
            m_backgroundActivatedArgs = backgroundActivatedEventArgs;
            m_backgroundActivatedArgs.TaskInstance.Canceled += TaskInstance_Canceled;
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            PersistentEventCancelationReason output = PersistentEventCancelationReason.Unknown;

            if(reason == BackgroundTaskCancellationReason.ExecutionTimeExceeded)
            {
                output = PersistentEventCancelationReason.ExecutionTimeExceeded;
            }
            else if(reason == BackgroundTaskCancellationReason.EnergySaver)
            {
                output = PersistentEventCancelationReason.EnergySaver;
            }
            else if(reason == BackgroundTaskCancellationReason.ResourceRevocation)
            {
                output = PersistentEventCancelationReason.ResourceRevocation;
            }
            else
            {
                output = PersistentEventCancelationReason.SystemPolicy;
            }

            Canceled(this, output);
        }

        private void CompleteDeferral()
        {
            m_defCount--;

            if(m_defCount == 0 && m_def != null)
            {
                m_def.Complete();
                m_selfRef = null;
            }
        }
    }

    public delegate void PersistentEventCancelationHandler(PersistentEventArgs sender, PersistentEventCancelationReason reason);

    public enum PersistentEventCancelationReason
    {
        Unknown = 0,
        ExecutionTimeExceeded,
        EnergySaver,
        ResourceRevocation,
        SystemPolicy
    }
}
