using System.Collections.Generic;

namespace BFSearchExample
{
    public interface IRules<TState>
    {
        int Size { get; }
        List<TState> Neighbors(TState currentState);
        int Distance(TState a, TState b);
        int H(TState state);
        bool IsTerminate(TState state);
    }
}
