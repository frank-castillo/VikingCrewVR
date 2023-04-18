using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StatefulObject : MonoBehaviour
{
    [System.Serializable]
    public class StateEntry
    {
        public string StateName;
        public GameObject StateObject;

        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(StateName) && StateObject != null; }
        }
    };

    [SerializeField]
    [GeneratedDropDownMenuAttribute(nameof(GetStateNames))]
    private string defaultStateName;
    [SerializeField] protected StateEntry currentState = null;
    [SerializeField] protected List<StateEntry> stateEntries = new List<StateEntry>(8);

    [SerializeField] protected bool SetDefaultStateOnBadStateRequest = false;

    public int StateCount
    {
        get { return stateEntries.Count; }
    }

    public StateEntry CurrentState
    {
        get { return currentState; }
    }

    public List<StateEntry> StateEntries
    {
        get { return stateEntries; }
    }

    public bool IsDefaultState { get { return currentState.StateName.Equals(defaultStateName, System.StringComparison.Ordinal); } }

    public string[] GetStateNames()
    {
        string[] stateNames = new string[StateEntries.Count];
        for (int i = 0; i < stateEntries.Count; ++i)
        {
            stateNames[i] = stateEntries[i].StateName;
        }
        return stateNames;
    }

    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved += HandleSceneSaving;
            return;
        }
#endif

        // Check to see if we're not currently in a state,
        // If we're not then we should set ourselves to the default state
        if (currentState == null || currentState.IsValid == false)
        {
            SetToDefaultState();
        }
    }

#if UNITY_EDITOR
    private void OnDisable()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.SceneManagement.EditorSceneManager.sceneSaved -= HandleSceneSaving;

            if (currentState != null && currentState.StateName != defaultStateName)
            {
                SetState(defaultStateName);
            }
        }
    }
#endif

    public void SetToDefaultState()
    {
        if (string.IsNullOrEmpty(defaultStateName))
        {
            if (StateEntries.Count > 0)
            {
                Debug.LogWarning(string.Format("No default state given to [{0}] - setting one now test", gameObject.name));
                defaultStateName = StateEntries[0].StateName;
                SetState(defaultStateName, true);
            }
        }
        else
        {
            SetState(defaultStateName, true);
        }
    }

    public void SetupValuesFromTarget(Transform pTarget)
    {
        pTarget.gameObject.SetActive(true);
        stateEntries.Clear();
        defaultStateName = null;

        // Go through the children of the target and register them as a state
        foreach (Transform t in pTarget)
        {
            string stateName = t.gameObject.name;
            stateEntries.Add(new StateEntry()
            {
                StateName = stateName,
                StateObject = t.gameObject
            });
        }

        if (stateEntries.Count > 0)
        {
            defaultStateName = stateEntries[0].StateName;
        }
    }

    public virtual bool SetState(string pState, bool pForce = false)
    {
        // Only change state when :
        // We are forced to, or
        // Our current state is invalid, or
        // The state we want is currently not the state we are in
        if (pForce == true || currentState == null || currentState.StateName?.Equals(pState, System.StringComparison.Ordinal) == false)
        {
            currentState = null;
            for (int i = stateEntries.Count - 1; i >= 0; --i)
            {
                bool isTarget = false;
                if (stateEntries[i] != null && stateEntries[i].StateName != null)
                {
                    isTarget = stateEntries[i].StateName.Equals(pState, System.StringComparison.Ordinal);
                }

                if (isTarget)
                {
                    currentState = stateEntries[i];
                }

                if (stateEntries[i].StateObject != null)
                {
                    stateEntries[i].StateObject.SetActive(isTarget);
                }
            }

            if (currentState == null)
            {
                Debug.LogWarning(string.Format("<b>[UI]</b> Couldn't find state named [{0}] on [{1}]. Setting to default state", pState, gameObject.name));

                if (!SetDefaultStateOnBadStateRequest)
                {
                    return false;
                }
                else
                {
                    if (string.Equals(pState, defaultStateName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                    else
                    {
                        return SetState(defaultStateName, pForce);
                    }
                }
            }
        }
        return true;
    }

    public void PublicSetState(string pState)
    {
        SetState(pState);
    }


    //single param version for button assignment.
    public virtual bool SetState(string pState)
    {
        return SetState(pState, false);
    }

    public bool HasState(string state)
    {
        for (int i = stateEntries.Count - 1; i >= 0; --i)
        {
            if (stateEntries[i].StateName.Equals(state, System.StringComparison.Ordinal))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsEntry(GameObject target)
    {
        for (int i = stateEntries.Count - 1; i >= 0; --i)
        {
            if (stateEntries[i].StateObject.GetInstanceID() == target.GetInstanceID())
            {
                return true;
            }
        }
        return false;
    }

    public StateEntry GetStateFromIndex(int pIndex)
    {
        if (pIndex < 0 || pIndex >= stateEntries.Count)
        {
            return null;
        }
        return stateEntries[pIndex];
    }

    public StateEntry GetStateFromName(string pStateName)
    {
        for (int i = stateEntries.Count - 1; i >= 0; --i)
        {
            if (stateEntries[i].StateName.Equals(pStateName, System.StringComparison.Ordinal))
            {
                return stateEntries[i];
            }
        }
        return null;
    }

    public int GetCurrentStateIndex()
    {
        if (currentState != null)
        {
            for (int i = stateEntries.Count - 1; i >= 0; --i)
            {
                if (!string.IsNullOrEmpty(stateEntries[i].StateName))
                {
                    if (stateEntries[i].StateName.Equals(currentState.StateName, System.StringComparison.Ordinal))
                    {
                        return i;
                    }
                }
            }
        }
        return -1;
    }

    public StateEntry GetCurrentState()
    {
        if (currentState != null)
        {
            for (int i = stateEntries.Count - 1; i >= 0; --i)
            {
                if (!string.IsNullOrEmpty(stateEntries[i].StateName))
                {
                    if (stateEntries[i].StateName.Equals(currentState.StateName, System.StringComparison.Ordinal))
                    {
                        return stateEntries[i];
                    }
                }
            }
        }
        return null;
    }

    public void SetToNextState()
    {
        SetState(GetStateFromIndex((GetCurrentStateIndex() + 1) % stateEntries.Count).StateName, true);
    }

    public void SetToRandomState()
    {
        SetState(GetStateFromIndex(UnityEngine.Random.Range(0, stateEntries.Count)).StateName, true);
    }

#if UNITY_EDITOR
    #region Editor Stuff

    /// <summary>
    /// We use this function to reset back to the defaultState when we save to avoid accidentally saving in the wrong unexpected state
    /// </summary>
    /// <param name="scene">The scene being saved, we only care about the scene we are in</param>
    private void HandleSceneSaving(UnityEngine.SceneManagement.Scene scene)
    {
        if (gameObject.scene == scene)
        {
            if (currentState.StateName != defaultStateName)
            {
                SetState(defaultStateName);
            }
        }
    }

    #endregion
#endif
}
