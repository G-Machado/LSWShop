using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntListener : VariableListener<int>
{
    public IntVariable variable;

    new void OnEnable()
    {
        Variable = variable;
        base.OnEnable();
    }

}