using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlashIt
{
    abstract public class StateTransitionTable
    {
        protected Dictionary<object, IState> table = new Dictionary<object, IState>();

        public void SetState(object evt, IState state)
        {
            table.Add(evt, state);
        }

        public IState GetState(object evt)
        {
            IState state = null;

            try
            {
                state = table[evt];

            }
            catch (KeyNotFoundException)
            {
                return null;
            }

            return state;
        }
    }
}
