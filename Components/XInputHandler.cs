using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XInputDotNetPure;
using System.Timers;
using UnityEngine;
using MU3.Sequence;

namespace UnityParrot.Components
{
    public class XInputHandler:MonoBehaviour
    {
        
        public GamePadState CurrentState { get; private set; }
        public GamePadState LastState { get; private set; }

        void Start()
        {

            CurrentState = GamePad.GetState(PlayerIndex.One);
            LastState = CurrentState;
        }

        void Update()
        {
            LastState = CurrentState;
            CurrentState = GamePad.GetState(PlayerIndex.One);

        }
        


    }
}
