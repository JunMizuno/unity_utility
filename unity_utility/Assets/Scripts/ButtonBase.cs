using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    private bool isTouched = false;
    private float[] buttonScaleRates = { };
    private float[] buttonScaleDurations = { };

    public void ButtonClickedFunc()
    {
        if (isTouched)
        {
            return;
        }

        isTouched = true;

        if(ButtonAnimation())
        {
            OnClick();
        }

        isTouched = false;
    }

    private bool ButtonAnimation()
    {
        return true;
    }

    virtual protected void OnClick()
    {
        
    }
}
