using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using UnityEditor;

namespace Ext.Debbug {
    public class DebugLogger {
        [Conditional("LOG_DEBUG")]
        public static void Log(string _message) {
            UnityEngine.Debug.Log(_message);
        }

        [Conditional("LOG_DEBUG")]
        public static void Logs<T>(IList<T> _list, Func<int, T, object> _function) {
            if (_function != null) {
                for (int i = 0; i < _list.Count; i++) {
                    UnityEngine.Debug.unityLogger.Log("", _function(i, _list[i]));
                }
            }
        }

        [Conditional("LOG_DEBUG")]
        public static void Logs(int _length, Func<int, object> _callback) {
            if (_callback != null) {
                for (int i = 0; i < _length; i++) {
                    UnityEngine.Debug.Log(_callback(i));
                }
            }
        }

        [Conditional("LOG_DEBUG")]
        public static void LogWarning(object _message) {
            UnityEngine.Debug.LogWarning(_message);
        }

        [Conditional("LOG_DEBUG")]
        public static void LogError(object _message) {
            UnityEngine.Debug.LogError(_message);
        }
    }
}
