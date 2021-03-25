using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class ReplaceFont : ScriptableObject
{
    public Font Target;
    public Font Replace;
}

public class FontReplacer : EditorWindow
{
    static SerializedProperty targetFont;
    static SerializedProperty replaceFont;

    [MenuItem("Utility/FontReplacer/Open")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(FontReplacer), true, "Font Replacer");
        var replacerObj = ScriptableObject.CreateInstance<ReplaceFont>();
        var serializedObjet = new UnityEditor.SerializedObject(replacerObj);
        targetFont = serializedObjet.FindProperty("Target");
        replaceFont = serializedObjet.FindProperty("Replace");
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(targetFont);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(replaceFont);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Included Target Font"))
        {
            if (targetFont.objectReferenceValue == null)
            {
                UnityEngine.Debug.Log("<color=yellow>" + "[Target] is not deside." + "</color>");
                return;
            }

            var textComponents = Resources.FindObjectsOfTypeAll(typeof(Text)) as Text[];
            int count = 0;
            foreach (var component in textComponents)
            {
                if (component.font == null)
                {
                    continue;
                }

                if (component.font.name == targetFont.objectReferenceValue.name)
                {
                    count++;

                    var obj = component.gameObject.transform;
                    if (obj.root)
                    {
                        UnityEngine.Debug.Log("<color=green>" + "Count:" + count + "  [Root]:" + obj.root + "</color>");
                    }

                    if (obj.parent)
                    {
                        UnityEngine.Debug.Log("<color=green>" + "Count:" + count + "  [Parent]:" + obj.parent + "</color>");
                    }
                }
            }

            if (count > 0)
            {
                UnityEngine.Debug.Log("<color=yellow>" + "Yes, it is included " + count + "pcs." + "</color>");
            }
            else
            {
                UnityEngine.Debug.Log("<color=yellow>" + "No, it is not included." + "</color>");
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Replace All Fonts"))
        {
            if (targetFont.objectReferenceValue == null)
            {
                UnityEngine.Debug.Log("<color=yellow>" + "[Target] is not deside." + "</color>");
                return;
            }

            if (replaceFont.objectReferenceValue == null)
            {
                UnityEngine.Debug.Log("<color=yellow>" + "[Replace] is not deside." + "</color>");
                return;
            }

            var textComponents = Resources.FindObjectsOfTypeAll(typeof(Text)) as Text[];
            int count = 0;
            foreach (var component in textComponents)
            {
                if (component.font == null)
                {
                    continue;
                }

                if (component.font.name == targetFont.objectReferenceValue.name)
                {
                    count++;

                    component.font = replaceFont.objectReferenceValue as Font;
                    UnityEngine.Debug.Log("<color=white>" + "Changed from [" + targetFont.objectReferenceValue.name + "]" + " to [" + replaceFont.objectReferenceValue.name + "]." + "</color>");
                }
            }

            if (count > 0)
            {
                // @todo. プレハブを操作したことを通知する処理が必要になる。
                // @todo. ヒエラルキー以外にも、プロジェクト内を検索できるようにしたい。
                EditorSceneManager.MarkAllScenesDirty();

                UnityEngine.Debug.Log("<color=cyan>" + "Changed Fonts " + count + "pcs." + "</color>");
            }
            else
            {
                UnityEngine.Debug.Log("<color=cyan>" + "Not Executed." + "</color>");
            }

            this.Close();
        }
    }
}
