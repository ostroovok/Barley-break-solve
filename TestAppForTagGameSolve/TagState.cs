using BFSearchExample;
using System.Text;

namespace TestAppForTagGameSolve
{
    public class TagState : State
    {
        public int SideSize { get; set; }
        private byte[] field;
        public byte[] Field
        {
            get => field;
            set
            {
                field = value;
                Hash = GetFieldHashCode();
            }
        }
        public TagState(State parent, int sideSize) : base(parent)
        {
            SideSize = sideSize;
        }
        public bool CheckState()
        {
            int N = 0;
            int e = 0;
            int sideSize = 4;
            for (int i = 0; i < Field.Length; i++)
            {
                /* Определяется номер ряда пустой клетки (считая с 1). */
                if (Field[i] == 0)
                    e = i / sideSize + 1;
                if (i == 0)
                    continue;
                /* Производится подсчет количества клеток меньших текущей */
                for (int j = i + 1; j < Field.Length; j++)
                    if (Field[j] < Field[i])
                        N++;
            }
            N += e;
            /* Если N является нечётной, то решения головоломки не существует. */
            return (N & 1) == 0; // Первый бит четного числа равен 0
        }
        public bool CheckState(byte[] field)
        {
            int N = 0;
            int e = 0;
            int sideSize = 4;
            for (int i = 0; i < field.Length; i++)
            {
                /* Определяется номер ряда пустой клетки (считая с 1). */
                if (field[i] == 0)
                    e = i / sideSize + 1;
                if (i == 0)
                    continue;
                /* Производится подсчет количества клеток меньших текущей */
                for (int j = i + 1; j < field.Length; j++)
                    if (field[j] < field[i])
                        N++;
            }
            N += e;
            /* Если N является нечётной, то решения головоломки не существует. */
            return (N & 1) == 0; // Первый бит четного числа равен 0
        }
        public override string ToString()
        {
            if (Field == null)
                return null;

            StringBuilder sbf = new StringBuilder(Field.Length);

            for (int i = 0; i < SideSize; i++)
            {
                for (int j = 0; j < SideSize; j++)
                    if (Field[j + i * SideSize] == 0)
                        sbf.Append($"|  |" + "\t");
                    else
                        sbf.Append($" {Field[j + i * SideSize]} " + "\t");

                sbf.Append('\n');
            }
            return sbf.ToString();
        }
        public string GetFieldHashCode()
        {
            var str = "";
            for (int i = 0; i < Field.Length; i++)
            {
                var r = Field[i] + 1;
                str += r.ToString();
            }
            return str;
        }
    }
}
