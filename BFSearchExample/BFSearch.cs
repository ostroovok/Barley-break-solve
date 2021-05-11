using System;
using System.Collections.Generic;
using System.Linq;

namespace BFSearchExample
{
    public class BFSearch<TState, TRules>
        where TState : State
        where TRules : IRules<TState>
    {
        public IRules<TState> Rules { get; }
        private FastPriorityQueue<TState> _open;
        public int Closed { get; private set; }

        public BFSearch(IRules<TState> rules)
        {
            Rules = rules ?? throw new Exception("Rules can't be null");
            _open = new(rules.Size);
        }

        public List<State> Search(TState startState)
        {
            _open.Clear();

            _open.Enqueue(startState, 0);
            List<string> close = new();

            startState.H = Rules.H(startState);

            TState x;

            while (_open.Count > 0)
            {
                x = _open.Dequeue();

                if (Rules.IsTerminate(x))
                    return Complete(x);

                close.Add(x.Hash);
                Closed++;
                
                bool betterH;
                
                var h = Rules.H(x);
                
                List<TState> neighbors = Rules.Neighbors(x);

                foreach (var n in neighbors)
                {
                    if (!close.Contains(n.Hash))
                    {
                        n.H = Rules.H(n);
                        _open.Enqueue(n, n.H);
                        betterH = true;
                    }
                    else
                        betterH = h <= n.H;

                    if (betterH)
                        n.Parent = x;
                }
            }
            return null;
        }

        private List<State> Complete(TState terminate)
        {
            Stack<State> path = new();
            State x = terminate;
            while (x != null)
            {
                path.Push(x);
                x = x.Parent;
            }
            return path.ToList();
        }
    }
}
