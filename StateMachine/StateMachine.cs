using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineLibrary
{
    public class StateMachine
    {
        private readonly IStateMachineReady _controller;
        private readonly List<StateMachimeTransition> _smtList = new();
        public StateMachine(IStateMachineReady controller)
        {
            _controller = controller;
        }

        public void AddTransition(int startState, int trigger, int endState, Action<object>? action)
        {
            _smtList.Add(new StateMachimeTransition(startState, trigger, endState, action));
        }

        public void HandleEvent(int trigger, object arguments)
        {
            var collection = _smtList.Where(t =>
                t.startState.CompareTo(_controller.State) == 0 && t.trigger.CompareTo(trigger) == 0);

            if (!collection.Any()) throw new ArgumentException($"State pair for [{_controller.State}, {trigger}] not found");
            if (collection.Count() > 1) throw new ArgumentException($"Too many state pair for [{_controller.State}, {trigger}]");

            var transition = collection.First();
            _controller.StateChange(transition.endState, transition.action, arguments);
        }
    }
}
