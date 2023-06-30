using UnityEngine;

public class ProgressEvaluation : MonoBehaviour
{
    [SerializeField] private float _requiredPercentage = 0.7f;
    [SerializeField] private bool _autoSucceed = false;
    private TierType _lastTierType = TierType.None;
    private int _successCounter = 0;
    private int _totalNotes = 0;
    private bool _repeatedTier = false;

    public void Prepare(TierType currentTierType, int totalNotes)
    {
        _totalNotes = totalNotes - 2;
        _successCounter = 0;

        _repeatedTier = _lastTierType == currentTierType ? true : false;
        _lastTierType = currentTierType;
    }

    public void Success()
    {
        ++_successCounter;
    }

    public void Fail()
    {
        if (_successCounter > 0)
        {
            --_successCounter;
        }
    }

    public bool MoveToNextTier()
    {
        float successPercentage = (float)_successCounter / (float)_totalNotes;

        Debug.Log($"Succesful Hits:[{_successCounter}] /  Total Note Count:[{_totalNotes}]");
        Debug.Log($"Success Percentage:[{successPercentage}]");

        if (_autoSucceed)
        {
            Debug.Log($"Tier Auto Complete");
            return true;
        }

        if (_repeatedTier)
        {
            Debug.Log($"Second Fail, Auto Pass");
            return true;
        }


        if (successPercentage > _requiredPercentage)
        {
            Debug.Log($"Tier Succeeded");
            return true;
        }

        Debug.Log($"Tier Failed, Restarting");
        return false;
    }
}
