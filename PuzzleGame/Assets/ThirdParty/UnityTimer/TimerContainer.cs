using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityTimer
{
    public class TimerContainer
    {
        public LinkedList<Timer> timers = new LinkedList<Timer>();

        public Timer AddLoopedTimer(float duration,
                                    Action onComplete,
                                    Action<float> onUpdate = null,
                                    bool useRealTime = false,
                                    MonoBehaviour autoDestroyOwner = null)
        {
            return this.AddTimer(duration,
                                 onComplete,
                                 onUpdate,
                                 true,
                                 useRealTime,
                                 autoDestroyOwner);
        }

        public Timer AddTimer(float duration,
                              Action onComplete,
                              Action<float> onUpdate = null,
                              bool isLooped = false,
                              bool useRealTime = false,
                              MonoBehaviour autoDestroyOwner = null)
        {
            var timer = Timer.Register(duration,
                                       onComplete,
                                       onUpdate,
                                       isLooped,
                                       useRealTime,
                                       autoDestroyOwner);
            this.timers.AddLast(timer);
            return timer;
        }

        public void CancelAllTimer()
        {
            foreach (Timer timer in this.timers)
            {
                timer.Cancel();
            }

            this.timers.Clear();
        }

        // Cancel and remove
        public void CancelTimer(Timer timer)
        {
            if (timer != null)
            {
                timer.Cancel();
                this.timers.Remove(timer);
            }
        }
    }
}