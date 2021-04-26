using BFSearchExample;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestAppForTagGameSolve
{
    public class TagRules : IRules<TagState>
    {
        public int SideSize { get; }
        public int Size { get; }
        public byte[] TerminateState { get; }
        public int[] Actions { get; set; }
        public int Left { get; private set; } = -1;
        public int Top { get; set; }
        public int Right { get; private set; } = 1;
        public int Bottom { get; set; }

        public TagRules(int sideSize, byte[] terminateState)
        {
            if (sideSize < 2)
                throw new Exception("Invalid field size");
            if (terminateState == null)
                throw new Exception("Terminate State can't be NULL");

            SideSize = sideSize;
            Size = SideSize * SideSize;

            if (terminateState.Length != Size)
                throw new Exception("Invalid terminate state length");
            TerminateState = terminateState;

            Top = -SideSize;
            Bottom = SideSize;

            Actions = new[] { Top, Bottom, Left, Right };
        }

        public int Distance(TagState a, TagState b)
        {
            State x = b;
            var result = 0;
            while ((x != null) && (!x.Equals(a)))
            {
                x = x.Parent;
                result++;
            }
            return result;
        }

        public int H(TagState state)
        {
            var resultOne = 0;
            var resultTwo = 0;
            for (int i = 0; i < Size; i++)
            {
                if (state.Field[i] != TerminateState[i])
                {
                    resultOne += Math.Abs(state.Field[i] - TerminateState[i]);
                    resultTwo++;
                }
            }
            resultOne += resultTwo;
            return resultOne + resultTwo;
        }

        public bool IsTerminate(TagState state) => state.Field.SequenceEqual(TerminateState);

        public List<TagState> Neighbors(TagState currentState)
        {
            List<TagState> res = new();
            for (int i = 0; i < Actions.Length; i++)
            {
                byte[] field = NextStep(currentState.Field, Actions[i]);
                if (field == null)
                {
                    continue;
                }
                TagState state = new(currentState, SideSize) { Field = field };
                res.Add(state);
            }
            return res;
        }

        public byte[] NextStep(byte[] field, int action)
        {
            /* Выполняется поиск пустой клетки */
            int zero = 0;
            for (; zero < field.Length; zero++)
            {
                if (field[zero] == 0)
                {
                    break;
                }
                if (zero >= field.Length)
                {
                    return null;
                }
            }
            /* Вычисляется индекс перемещаемой клетки */
            int number = zero + action;
            /* Проверяется допустимость хода */
            if (number < 0 || number >= field.Length)
            {
                return null;
            }
            if ((action == 1) && ((zero + 1) % SideSize == 0))
            {
                return null;
            }
            if ((action == -1) && ((zero + 1) % SideSize == 1))
            {
                return null;
            }
            /*
             * Создается новый экземпляр поля, на котором меняются местами пустая и
             * перемещаемая клетки
             */
            var newField = new byte[field.Length];
            Array.Copy(field, newField, field.Length);
            var temp = newField[zero];
            newField[zero] = newField[number];
            newField[number] = temp;

            return newField;
        }
    }
}
