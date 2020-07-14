using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoBehaviour
{
    public List<MindMapData> states;
    public int currentStateIndex = -1;

    public void Push(MindMapData state)
    {
        if (currentStateIndex != -1 && currentStateIndex < states.Count)
        {
            states.RemoveRange(currentStateIndex + 1, states.Count - 1 - currentStateIndex);
        }

        states.Add(state);
        currentStateIndex++;

        Debug.Log("New State Saved");
    }

    public void Pop()
    {
        states.RemoveAt(states.Count - 1);
    }

    public MindMapData GetCurrentState()
    {
        return states[currentStateIndex];
    }
}
