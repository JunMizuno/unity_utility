using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour
{
    public Text fpsText;

    private float fps;
    private int frames;
    private float transitTimes;
    private bool firstTime = true;

    public void Start()
    {
        SetFPSTextPosition();
    }

    public void Update()
    {
        if (firstTime)
        {
            firstTime = false;
            frames = 0;
            transitTimes = Time.realtimeSinceStartup;
            return;
        }

        frames++;
        float deltaTime = Time.realtimeSinceStartup - transitTimes;
        if (deltaTime > 1.0f && frames > 10)
        {
            RefreshFPSText();

            transitTimes = Time.realtimeSinceStartup;
            frames = 0;
        }
    }

    // [Edit]->[ProjectSettings]->[Time]->[TimeManager]->[Fixed Timestep]
    public void FixedUpdate()
    {

    }

    public void SetFPSTextPosition()
    {
        if (!fpsText)
        {
            return;
        }

        float screenLeft = -(Screen.width / 2.0f) + (fpsText.rectTransform.sizeDelta.x / 2.0f);
        float screenBottom = -(Screen.height / 2.0f) + (fpsText.rectTransform.sizeDelta.y / 2.0f);
        var pos = fpsText.rectTransform.anchoredPosition;
        pos.x = screenLeft;
        pos.y = screenBottom;
        fpsText.rectTransform.anchoredPosition = pos;
    }

    public void RefreshFPSText()
    {
        if (!fpsText)
        {
            return;
        }

        fpsText.text = string.Format("FPS:{0}", frames);
    }
}
