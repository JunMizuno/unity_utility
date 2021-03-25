using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneController
{
    public enum SCENE_NUM
    {
        None = -1,
        GameController = 0,
        TestShaderScene,
        SendMailScene,
    }

    public static readonly string[] SceneNames = {
        "None",
        "GameController",
        "TestShaderScene",
    };
}
