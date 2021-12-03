using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public enum EffectType
    {

    }

    public class Effects : MonoBehaviour
    {
        private static Effects _instance = null;
        public static Effects Instance
        {
            get
            {
                object instance = FindObjectOfType(typeof(Effects));
                if (instance != null) _instance = (Effects)instance;
                else
                {
                    GameObject obj = new GameObject("Effects");
                    _instance = obj.AddComponent<Effects>();
                    obj.hideFlags = HideFlags.HideInHierarchy;
                }

                return _instance;
            }
        }


    }
}

