using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolListener : VariableListener<bool>
{
    public BoolVariable variable;

    new void OnEnable()
    {
        Variable = variable;
        base.OnEnable();
    }

}
