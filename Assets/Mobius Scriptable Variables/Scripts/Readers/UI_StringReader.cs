using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StringReader : MonoBehaviour
{
    private Text UIText;
    public StringVariable variable;

    void Start()
    {
        UIText = GetComponent<Text>();
    }

    void Update()
    {
        if(UIText)
            UIText.text = variable.Value;
    }
}
