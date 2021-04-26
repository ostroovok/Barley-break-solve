using BFSearchExample;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestAppForTagGameSolve
{
    class Program
    {
        private static byte[] startField;

        private static byte[] terminateField;

        private static int _stepCount = 10;

        private static int _sideSize = 4;
        private static Random _rnd;

        public async static void Main(string[] args)
        {
            _rnd = new();
            int size = _sideSize * _sideSize;
            terminateField = GetTerminalState(_sideSize, size);

            TagRules rules = new TagRules(_sideSize, terminateField);
            TagState startState = new TagState(null, _sideSize);

            if (startField == null)
                startField = GenerateStartState(rules, _stepCount);
            var counter = 0;

            while (!startState.CheckState(startField))
            {
                startField = GenerateStartState(rules, _stepCount);
                Console.SetCursorPosition(0, 0);
                switch (counter % 3)
                {
                    case 0:
                        Console.WriteLine("Generating a solvable combination \n Wait please. ");
                        break;
                    case 1:
                        Console.WriteLine("Generating a solvable combination \n Wait please.. ");
                        break;
                    case 2:
                        Console.WriteLine("Generating a solvable combination \n Wait please... ");
                        break;
                }
                counter++;
            }

            startState.Field = startField;

            BFSearch<TagState, TagRules> BFS = new(rules);
            
            List<State> res = await BFSearshAsync(BFS, startState);

            counter = 0;
            while (res == null)
            {
                switch (counter % 3)
                {
                    case 0:
                        Console.WriteLine("Solve in progress \n Wait please. ");
                        break;
                    case 1:
                        Console.WriteLine("Solve in progress \n Wait please.. ");
                        break;
                    case 2:
                        Console.WriteLine("Solve in progress \n Wait please... ");
                        break;
                }
                counter++;
            }
            //Console.Clear();
            Print(res);
            Console.WriteLine($"\n Count closed: {BFS.Closed}");
        }
        #region Private Methods
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
            foreach (var s in states)
            {
                Console.WriteLine(s.ToString());
            }
        }
    }
}
