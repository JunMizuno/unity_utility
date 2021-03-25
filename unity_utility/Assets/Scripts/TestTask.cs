using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

public class TestTask : MonoBehaviour
{
    private CancellationTokenSource tokenSource_;

    private Dictionary<Texture2D, Texture2D> textureCache = new Dictionary<Texture2D, Texture2D>();

    private void OnEnable()
    {
        if (tokenSource_ == null)
        {
            tokenSource_ = new CancellationTokenSource();
        }
        DoTest();
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        if (tokenSource_ != null)
        {
            tokenSource_.Cancel();
        }
        Debug.Log("------破棄された");
    }

    public async Task TestFunction(CancellationToken cancelToken)
    {
        LogOutput("------Start");

        /*
        await Task.Delay(5000);
             
        if (cancelToken.IsCancellationRequested)
        {
            return;
        }

        LogOutput($"テスト処理");
        */

        // それぞれDelay
        /*
        for (int i = 0; i < 10; i++)
        {
            await Task.Delay(1000).ContinueWith(_ =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                LogOutput($"テスト処理 カウント:{i}");
            });
        }
        */

        Thread.Sleep(1000);

        for (int i = 0; i < 10; i++)
        {
            await Task.Run(() =>
            {
                if (cancelToken.IsCancellationRequested)
                {
                    return;
                }

                LogOutput($"テスト処理 カウント:{i}");
            });
        }

        if (cancelToken.IsCancellationRequested)
        {
            return;
        }

        LogOutput("------End");
    }

    public void DoTest()
    {
        var cancelToken = tokenSource_.Token;
        Task.Run(() => TestFunction(cancelToken));
    }

    private void LogOutput(string message)
    {
        Debug.Log(DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "\t" + "スレッドナンバー:" + Thread.CurrentThread.ManagedThreadId + "\t" + message);
    }

    public Texture2D CreateBlurTexture(Texture2D tex, float sig, bool isCache = true)
    {
        if (isCache && textureCache.ContainsKey(tex))
        {
            return textureCache[tex];
        }

        int width = 0;
        int height = 0;






        var createTexture = new Texture2D(width, height);
        //createTexture.SetPixel(dst);
        //createTexture.Apply();

        return createTexture;
    }

    public void Release()
    {
        textureCache.Clear();
    }
}
