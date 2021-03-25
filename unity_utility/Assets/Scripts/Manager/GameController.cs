using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    private static readonly string CLASS_NAME = "GameController";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void FirstLoad()
    {
#if UNITY_EDITOR
        // @memo.mizuno string.CompareTo()を使用すると、"カルチャ対応"アラートが出たため下記のようにした。
        if (string.Compare(SceneManager.GetActiveScene().name, CLASS_NAME, System.StringComparison.CurrentCulture) != 0)
        {
            SceneManager.LoadScene(CLASS_NAME);
        }
#endif
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (!this.enabled)
        {
            return;
        }

        GameController.instance = this;
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        // @memo.mizuno 最初に起動させるシーンをここで指定する。
        // @memo.mizuno LoadSceneMode.Additiveだとシーンが重複して生成されてしまう。(対象のシーンがDontDestroyOnLoadを含んでいる場合に限る。)
        SceneManager.LoadSceneAsync((int)SceneController.SCENE_NUM.TestShaderScene, LoadSceneMode.Single);
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {

    }

    public void OnDestroy()
    {
        GameController.instance = null;
    }

    public void OnApplicationFocus(bool focus)
    {
        
    }

    public void OnApplicationPause(bool pause)
    {
        
    }

    public void OnApplicationQuit()
    {
        
    }
}
