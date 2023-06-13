using UnityEngine;

public class ProgressEvaluation : MonoBehaviour
{
    [SerializeField] private float _requiredPercentage = 0.7f;
    private int _successCounter = 0;
    private int _totalNotes = 0;

    public void Prepare(int totalNotes)
    {
        _totalNotes = totalNotes;
        _successCounter = 0;
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
        float successPercentage = _successCounter / _totalNotes;

        if (successPercentage > _requiredPercentage)
        {
            return true;
        }

        return false;
    }
}
