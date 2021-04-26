namespace BFSearchExample
{
    public abstract class State
    {
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
        public int H { get; set; }
        public State Parent { get; set; }
        public int QueueIndex { get; set; }
        public virtual string Hash { get; set; }
        protected State(State parent) => Parent = parent;
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
