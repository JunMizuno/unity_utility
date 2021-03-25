using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderController : MonoBehaviour
{
    private float transitTimes;
    private float alphaFrames;
    private float alphaDirect = 1.0f;

    public void Start()
    {
        transitTimes = 0.0f;

        SetSurfaceBaseColor();
    }

    public void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - transitTimes;
        if (deltaTime >= 1.0f)
        {
            transitTimes = Time.realtimeSinceStartup;

            SetSurfaceBaseColor();
        }
    }

    public void SetSurfaceBaseColor()
    {
        if (!GetComponent<Renderer>())
        {
            return;
        }

        /*
        float rate = Time.time * 2.0f;
        float r = Mathf.Sin(rate);
        if (r < 0.0f)
        {
            r *= -1.0f;
        }
        float g = Mathf.Cos(rate);
        if (g < 0.0f)
        {
            g *= -1.0f;
        }
        float b = Mathf.Sin(rate) / 2.0f;
        if (b < 0.0f)
        {
            b *= -1.0f;
        }
        */

        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        float a = Random.Range(0.0f, 1.0f);
        a = Time.realtimeSinceStartup % 1.0f;

        GetComponent<Renderer>().material.SetColor("_BaseColor", new Color(r, g, b, a));
    }
}
