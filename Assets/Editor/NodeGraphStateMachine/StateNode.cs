using GraphProcessor;
using System;
using StateMachine;

[Serializable, NodeMenuItem("StateMachine/State")]
public class StateNode : BaseNode
{
    [Input(name = "InputState")] public State _inputState;
    [Output(name = "NextState")] public State _nextState;

    public override string name => "State";
    protected override void Process() => _nextState = _inputState;
}
