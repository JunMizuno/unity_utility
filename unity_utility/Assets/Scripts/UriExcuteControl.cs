using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UriExcuteControl : MonoBehaviour {
    // @todo. 特殊動作をしたいキーワードをURL内に設定する
    private const string CHECK_KEY = "test";

    // @todo. 動作させたい処理のキーワードを設定する
    private const string SHOW_POPUP = "show_popup";
    private const string REPLACE_SCENE = "relpace_scene";
    private const string DIGIT_HTTP = "http";
    private const string DIGIT_HTTPS = "https";

    /// <summary>
    /// 指定されたURIによって各処理を実行
    /// </summary>
    /// <param name="_uri">URI.</param>
    /// <param name="_callback">Callback.</param>
    public static void OpenUri(Uri _uri, Action<bool> _callback = null) {
        var scheme = _uri.Scheme.ToLower();
        if (scheme == CHECK_KEY) {
            string host = _uri.Host.ToLower();
            switch (host) {
                case SHOW_POPUP:
                    break;

                case REPLACE_SCENE:
                    break;
            }
        }
        else if (scheme == DIGIT_HTTP || scheme == DIGIT_HTTPS) {
            // @todo. ブラウザを開く処理

            if (_callback != null) {
                _callback(true);
            }
        }
        else {
            Debug.Log("URI設定がイレギュラー");
        }
    }
}
