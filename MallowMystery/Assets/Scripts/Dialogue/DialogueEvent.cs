using System.Collections;
using System.Collections.Generic;
using Dialogue.RunTime;
using Subtegral.DialogueSystem.DataContainers;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/DialogueEvent")]
public class DialogueEvent : ScriptableObject
{
    private List<DialogueListener> _listeners = new List<DialogueListener>();
    public DialogueContainer DialogueContainer = null;
    public void Raise(DialogueContainer dialogue)
    {
        for (int i = _listeners.Count -1; i >= 0; i--)
        {
            if (DialogueContainer != null) {
                _listeners[i].OnEventTriggered(dialogue);
            }
            else {
                _listeners[i].OnEventTriggered(dialogue);
            }
        }
    }
    public void AddListener(DialogueListener listener)
    {
        _listeners.Add(listener);
    }
    public void RemoveListener(DialogueListener listener)
    {
        _listeners.Remove(listener);
    }
}
