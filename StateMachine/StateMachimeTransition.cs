using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
    internal class StateMachimeTransition
    {
        public readonly int startState;
        public readonly int trigger;
        public readonly int endState;
        public readonly Action<object>? action;

        public StateMachimeTransition(int startState, int trigger, int endState, Action<object>? action = null)
        {
            this.startState = startState;
            this.trigger = trigger;
            this.endState = endState;
            this.action = action;
        }
    }
}
