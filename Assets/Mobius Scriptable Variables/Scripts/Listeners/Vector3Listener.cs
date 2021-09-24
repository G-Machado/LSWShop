using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Vector3Listener : VariableListener<Vector3>
{
    public Vector3Variable variable;

    new void OnEnable()
    {
        Variable = variable;
        base.OnEnable();
    }

}
