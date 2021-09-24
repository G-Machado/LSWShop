using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringListener : VariableListener<string>
{
    public StringVariable variable;

    new void OnEnable()
    {
        Variable = variable;
        base.OnEnable();
    }

}
