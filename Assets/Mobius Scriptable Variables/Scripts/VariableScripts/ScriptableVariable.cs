using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ScriptableVariable<Type> : ScriptableObject, ISerializationCallbackReceiver
{
    public Type Value;

    [System.NonSerialized]
    public Type RunTimeValue;

    [System.NonSerialized]

    private List<VariableListener<Type>> listeners = new List<VariableListener<Type>>();

    public void ValueChanged()
    {
        for(int i = listeners.Count -1; i >= 0; i--)
            listeners[i].OnValueChanged(this);
    }

    public void RegisterListener(VariableListener<Type> listener)
    { 
        listeners.Add(listener); 
    }

    public void UnregisterListener(VariableListener<Type> listener)
    { 
        listeners.Remove(listener); 
    }

    public void OnAfterDeserialize()
    {
        RunTimeValue = Value;
    }

    public void OnBeforeSerialize() 
    { 

    }

    public void SetValue(Type value)
    {
        Value = value;

        ValueChanged();
    }

    public void SetRuntimeValue(Type value)
    {
        RunTimeValue = value;
        ValueChanged();
    }

}
