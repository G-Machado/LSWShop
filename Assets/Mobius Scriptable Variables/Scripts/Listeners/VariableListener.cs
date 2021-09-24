using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VariableListener<Type> : MonoBehaviour
{
    public ScriptableVariable<Type> Variable;
    public UnityEvent OnVariableChange;

    protected void OnEnable()
    { 
        Variable.RegisterListener(this); 
    }

    protected void OnDisable()
    { 
        Variable.UnregisterListener(this); 
    }

    public virtual void OnValueChanged(ScriptableVariable<Type> variable)
    {
        OnVariableChange.Invoke();
        //Debug.Log("value changed!");
    }
}
