namespace BFSearchExample
{
    public abstract class State
    {
       
        public int H { get; set; }
        public State Parent { get; set; }
        public virtual string Hash { get; set; }
        public int QueueIndex { get; set; }
        protected State(State parent) => Parent = parent;
        
    }
}
