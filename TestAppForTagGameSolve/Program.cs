using BFSearchExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestAppForTagGameSolve
{
    class Program
    {
        private static byte[] startField;

        private static byte[] terminateField;

        private static int _stepCount = 10;

        private static int _sideSize = 4;

        private static BFSearch<TagState, TagRules> _BFS;

        private static List<State> _res;

        private static Random _rnd;

        public static void Main(string[] args)
        {
            Solve();
            Console.Clear();
            var counter = 0;
            while (_res == null)
            {
                if (counter > 15)
                    throw new Exception("Timeout exceeded");
                Console.SetCursorPosition(0, 0);
                WaitMes(counter, "Solve in process");
                counter++;
                Thread.Sleep(1000);
            }
            Print(_res);
        }
        
        public async static void Solve()
        {
            _rnd = new();
            int size = _sideSize * _sideSize;
            terminateField = GetTerminalState(_sideSize, size);

            TagRules rules = new TagRules(_sideSize, terminateField);
            TagState startState = new TagState(null, _sideSize);

            if (startField == null)
                startField = GenerateStartState(rules, _stepCount);
            var counter = 0;

            while (!startState.CheckState(startField) || startField.SequenceEqual(terminateField))
            {
                startField = GenerateStartState(rules, _stepCount);
                Console.SetCursorPosition(0, 0);
                WaitMes(counter, "Generating a acceptable combination");
                counter++;
            }

            startState.Field = startField;

            _BFS = new(rules);
            
            _res = await BFSearshAsync(_BFS, startState);
        }
        #region Private Methods
        private static void WaitMes(int counter, string str)
        {
            switch (counter % 3)
            {
                case 1:
                    Console.WriteLine($"{str} \n Wait please. ");
                    break;
                case 2:
                    Console.WriteLine($"{str} \n Wait please.. ");
                    break;
                case 0:
                    Console.WriteLine($"{str} \n Wait please... ");
                    break;
            }
        }
        private static async Task<List<State>> BFSearshAsync(BFSearch<TagState, TagRules> BFS, TagState startState)
        {
            return await Task.Run(() => BFS.Search(startState));
        }
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
            Console.Clear();
            foreach (var s in states)
            {
                Console.WriteLine(s.ToString());
            }
            Console.WriteLine($"\n---Closed: {_BFS.Closed}---");
        }
    }
}
