using UnityEngine;

public class VikingBehavior : MonoBehaviour
{
    // Core AI
    private CrewController _crewController = null;
    private VikingSleepState _vikingSleepState = null;
    private VikingRowState _vikingRowState = null;
    private IVikingState _currentState = null;

    // Animations
    private Animator _animator = null;
    private VikingAnimationType _currentAnimationType = VikingAnimationType.None;
    private string _idleTrigger = "Idle";
    private string _stretchTrigger = "Arm Stretch";
    private string _yawnTrigger = "Yawn";

    public VikingAnimationType CurrentAnimationType { get => _currentAnimationType; }

    private void SetRowingState(bool isRowing) { _animator.SetBool("Rowing", isRowing); }

    public void Initialize(CrewController crewController, float sleepDelay, float sleepVariance)
    {
        _crewController = crewController;

        _animator = GetComponent<Animator>();

        _vikingSleepState = new VikingSleepState(this, sleepDelay, sleepVariance);
        _vikingRowState = new VikingRowState(this);
    }

    private void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
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

        _crewController.SetPaddleAnimation(rowType);
    }

    public void AnimationState(VikingAnimationType animationType)
    {
        _currentAnimationType = animationType;

        switch (animationType)
        {
            case VikingAnimationType.Idle:
                SetRowingState(false);
                break;
            case VikingAnimationType.Row:
                SetRowingState(true);
                break;
            default:
                Enums.InvalidSwitch(GetType(), animationType.GetType());
                break;
        }
    }

    public void IdleAnimationTriggers(VikingAnimationType animationType)
    {
        _currentAnimationType = animationType;

        switch (animationType)
        {
            case VikingAnimationType.Idle:
                _animator.SetTrigger(_idleTrigger);
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
