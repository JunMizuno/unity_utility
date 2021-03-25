using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @memo. Task使用で追記
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class AsyncBase : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns>The enabled task.</returns>
    public async Task OnEnabledTask()
    {
        await Task.Run(() =>
        {
            OnEnabled();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>The update.</returns>
    public async Task OnUpdateTask()
    {
        await Task.Run(() => 
        {
            OnUpdate();
        });
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnEnabled()
    {

    }

    /// <summary>
    /// onUpdate
    /// </summary>
    protected virtual void OnUpdate()
    {

    }
}
