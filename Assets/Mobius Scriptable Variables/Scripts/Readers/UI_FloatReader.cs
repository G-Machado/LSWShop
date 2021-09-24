using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FloatReader : MonoBehaviour
{

    private Text UIText;
    public FloatVariable variable;

    void Start()
    {
        UIText = GetComponent<Text>();
    }

    void Update()
    {
        if(UIText)
            UIText.text = variable.Value.ToString();
    }
}
