using System;
using UnityEngine;

namespace Game.Tools
{

    public class Timer : MonoBehaviour
    {
        private bool working;
        private float time;
        private Action callBack;

        public void StartTimer(float time, Action callBack)
        {
            this.time = time;
            this.callBack = callBack;
            working = true;
        }

        public void StopTimer()
        {
            working = false; 
        }

        void Update()
        {
            if (working)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    StopTimer();
                    callBack();
                }
            }
        }
    }

}
