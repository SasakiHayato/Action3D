using UnityEngine.UIElements;
using UnityEditor.UIElements;
using GraphProcessor;
using StateMachine;

[NodeCustomEditor(typeof(StateNode))]
public class StateView : BaseNodeView
{
    public override void Enable()
    {
        StateNode stateNode = nodeTarget as StateNode;

        ObjectField objectField = new ObjectField()
        {
            objectType = typeof(State),
            value = stateNode._inputState,
        };

        controlsContainer.Add(objectField);
    }
}
