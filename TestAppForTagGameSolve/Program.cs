using BFSearchExample;
using System;
using System.Collections.Generic;

namespace TestAppForTagGameSolve
{
    class Program
    {
        private static byte[] startField;

        private static byte[] terminateField;

        private static int _stepCount = 10;

        private static int _sideSize = 2;
        private static Random _rnd;

        public static void Main(string[] args)
        {
            _rnd = new();
            int size = _sideSize * _sideSize;
            terminateField = GetTerminalState(_sideSize, size);

            TagRules rules = new TagRules(_sideSize, terminateField);
            TagState startState = new TagState(null, _sideSize);

            if (startField == null)
            {
                startField = GenerateStartState(rules, _stepCount);
            }
            startState.Field = startField;

            if (!startState.CheckState())
                throw new Exception("Unsolvable combination");


            BFSearch<TagState, TagRules> BFS = new(rules);

            List<State> res = BFS.Search(startState);
            Print(res);
            Console.WriteLine($"\n Count closed: {BFS.Closed}");
        }
        #region Private Methods
        private static byte[] GenerateStartState(TagRules rules, int swapCount)
        {
            int stepCount = swapCount;
            byte[] startState = rules.TerminateState;

            int[] actions = rules.Actions;
            while (stepCount > 0)
            {
                int j = _rnd.Next(actions.Length);
                byte[] state = rules.NextStep(startState, actions[j]);
                if (state != null)
                {
                    startState = state;
                    stepCount--;
                }
            }
            return startState;
        }
        private static byte[] GetTerminalState(int sideSize, int size)
        {
            if (terminateField == null)
            {
                terminateField = new byte[size];
                byte k = 0;
                for (int i = 0; i < sideSize; i++)
                {
                    for (int j = 0; j < sideSize; j++)
                    {
                        terminateField[j + i * sideSize] = ++k;
                    }
                }
                terminateField[size - 1] = 0;
            }
            return terminateField;
        }
        #endregion

        public static void Print(List<State> states)
        {
            foreach (var s in states)
            {
                Console.WriteLine(s.ToString());
            }
        }
    }
}
