using UnityEngine;

public class VikingBehavior : MonoBehaviour
{
    private Animator _animator = null;
    private VikingSleepState _vikingSleepState = null;
    private VikingRowState _vikingRowState = null;

    private IVikingState _currentState = null;

    public Animator Animator { get => _animator; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _animator = GetComponent<Animator>();

        _vikingSleepState = new VikingSleepState();
        _vikingRowState = new VikingRowState();
    }

    private void ChangeState(IVikingState vikingState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = vikingState;

        _currentState.Enter(this);
    }

    public void RowEvent(RowType rowType)
    {
        switch (rowType)
        {
            case RowType.StartRowing:
                ChangeState(_vikingRowState);
                break;
            case RowType.StopRowing:
                ChangeState(_vikingSleepState);
                break;
            default:
                Enums.InvalidSwitch(GetType(), rowType.GetType());
                break;
        }
    }
}
