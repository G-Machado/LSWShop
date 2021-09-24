using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FloatListener : VariableListener<float>
{
    public FloatVariable variable;

    new void OnEnable()
    {
        Variable = variable;
        base.OnEnable();
    }

}
