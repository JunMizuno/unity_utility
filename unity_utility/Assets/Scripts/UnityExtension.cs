using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ext.Unity3D {
    public static class UnityExtension {
        public static float Floor(float _baseValue, int _digit) {
            // 何桁目以降を切り捨てるか
            float power = Mathf.Pow(10, _digit);
            return Mathf.Floor(_baseValue * power) / power;
        }

        public static T CreateObject<T>(string _name, Transform _parent, Vector3 _localPos, Vector3 _localScale, Quaternion _localRotation) where T : Component {
            GameObject gameObject = new GameObject();
            Transform transform = gameObject.transform;
            transform.SetParent(_parent);
            transform.localPosition = _localPos;
            transform.localScale = _localScale;
            transform.localRotation = _localRotation;
            T component = gameObject.AddComponent<T>();
            return component;
        }

        public static T CreateObject<T>(string _name) where T : Component {
            return UnityExtension.CreateObject<T>(_name, null, Vector3.zero, Vector3.one, Quaternion.identity);
        }

        public static T CreateObject<T>(string _name, Transform _parent) where T : Component {
            return UnityExtension.CreateObject<T>(_name, _parent, Vector3.zero, Vector3.one, Quaternion.identity);
        }

        public static T CreateObject<T>(string _name, Transform _parent, Vector3 _localPos) where T : Component {
            return UnityExtension.CreateObject<T>(_name, _parent, _localPos);
        }

        public static T CreateObject<T>(string _name, Transform _parent, Vector3 _localPos, Vector3 _localScale) where T : Component {
            return UnityExtension.CreateObject<T>(_name, _parent, _localPos, _localScale, Quaternion.identity);
        }

        public static T CreateObject<T>(string _name, Transform _parent, Quaternion _localRotation) where T : Component {
            return UnityExtension.CreateObject<T>(_name, _parent, Vector3.zero, Vector3.one, _localRotation);
        }

        public static T CreateObject<T>(string _name, Vector3 _localPos) where T : Component {
            return UnityExtension.CreateObject<T>(_name, null, _localPos, Vector3.one, Quaternion.identity);
        }

        public static T CreateObject<T>(string _name, Vector3 _localPos, Vector3 _localScale) where T :Component {
            return UnityExtension.CreateObject<T>(_name, null, _localPos, _localScale, Quaternion.identity);
        } 

        public static T CreateObject<T>(string _name, Quaternion _localRotation) where T : Component {
            return UnityExtension.CreateObject<T>(_name, null, Vector3.zero, Vector3.one, _localRotation);
        }





    }
}
