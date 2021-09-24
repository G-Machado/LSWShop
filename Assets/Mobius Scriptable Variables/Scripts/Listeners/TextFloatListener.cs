using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFloatListener : VariableListener<float>
{
    private Text myText;
    
    public FloatVariable floatVar;

    new void OnEnable()
    {
        Variable = floatVar;
        base.OnEnable();
    }

    public override void OnValueChanged(ScriptableVariable<float> variable)
    {
        myText.text = variable.Value.ToString();

        base.OnValueChanged(variable);
    }

    void Start()
    {
        myText = GetComponent<Text>();

        if(floatVar)
            myText.text = floatVar.Value.ToString();
    }
}
