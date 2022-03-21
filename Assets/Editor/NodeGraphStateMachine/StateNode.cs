using GraphProcessor;
using System;
using StateMachine;

[Serializable, NodeMenuItem("State")]
public class StateNode : BaseNode
{
    [Input(name = "Entry")] public State _entryState;
    [Output(name = "Exit")] public State _exitState;


}
