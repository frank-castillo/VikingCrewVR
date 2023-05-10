public class VikingRowState : IVikingState
{
    private VikingBehavior _behavior = null;

    public VikingRowState(VikingBehavior vikingBehavior)
    {
        _behavior = vikingBehavior;
    }

    public void Enter()
    {
        _behavior.ChangeAnimation(VikingAnimationType.Push);
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}