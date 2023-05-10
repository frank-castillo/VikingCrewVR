using UnityEngine;

public class VikingBehavior : MonoBehaviour
{
    // Core AI
    private VikingSleepState _vikingSleepState = null;
    private VikingRowState _vikingRowState = null;
    private IVikingState _currentState = null;

    // Animations
    private Animator _animator = null;
    private VikingAnimationType _currentAnimationType = VikingAnimationType.None;
    private string _idleTrigger = "Idle";
    private string _pushTrigger = "Yawn";
    private string _stretchTrigger = "ArmStretch";
    private string _yawnTrigger = "Row";

    public Animator Animator { get => _animator; }
    public VikingAnimationType CurrentAnimationType { get => _currentAnimationType; }

    public void Initialize(float sleepDelay, float sleepVariance)
    {
        _animator = GetComponent<Animator>();

        _vikingSleepState = new VikingSleepState(this, sleepDelay, sleepVariance);
        _vikingRowState = new VikingRowState();
    }

    public void StartDefaultBehavior()
    {
        ChangeState(_vikingSleepState);
    }

    private void ChangeState(IVikingState vikingState)
    {
        if (vikingState == _currentState)
        {
            return;
        }

        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = vikingState;

        _currentState.Enter();
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

    public void ChangeAnimation(VikingAnimationType animationType)
    {
        _currentAnimationType = animationType;

        switch (animationType)
        {
            case VikingAnimationType.Idle:
                _animator.SetTrigger(_idleTrigger);
                break;
            case VikingAnimationType.Push:
                _animator.SetTrigger(_pushTrigger);
                break;
            case VikingAnimationType.Stretch:
                _animator.SetTrigger(_stretchTrigger);
                break;
            case VikingAnimationType.Yawn:
                _animator.SetTrigger(_yawnTrigger);
                break;
            default:
                Enums.InvalidSwitch(GetType(), animationType.GetType());
                break;
        }
    }
}
