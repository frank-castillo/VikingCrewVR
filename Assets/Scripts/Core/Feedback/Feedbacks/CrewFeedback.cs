using System.Collections.Generic;
using UnityEngine;

public class CrewFeedback : Feedback
{
    [SerializeField] private RowType _rowingType = RowType.None;
    private List<VikingBehavior> _vikings = null;

    public void SetVikings(List<VikingBehavior> vikingBehaviors) { _vikings = vikingBehaviors; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Play()
    {
        base.Play();

        foreach (VikingBehavior viking in _vikings)
        {
            viking.RowEvent(_rowingType);
        }
    }

    public override void Stop()
    {
        base.Stop();
    }
}