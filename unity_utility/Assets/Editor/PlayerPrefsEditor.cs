using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsEditor : MonoBehaviour
{
    [MenuItem ("Utility/PlayerPrefs/DeleteAll")]
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll ();
    }
}
