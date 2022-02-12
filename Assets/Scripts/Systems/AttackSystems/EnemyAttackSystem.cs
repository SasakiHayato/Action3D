using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttackSetting
{
    public class EnemyAttackSystem : MonoBehaviour
    {
        public bool MasterRequest { get; private set; } = false;
        public bool Request { get; private set; } = false;

        public bool SetMasterRequest(bool set) => MasterRequest = set;
        public bool SetRequest(bool set) => Request = set;

        public void SetWarningEffect(Transform transform)
        {
            GameObject obj = Instantiate((GameObject)Resources.Load(""));
            obj.transform.position = transform.position;
        }
    }
}
