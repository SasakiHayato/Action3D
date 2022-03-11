using UnityEngine;

/// <summary>
/// Enemy��AI��BeheviaTree�ƕ��p������N���X
/// </summary>

namespace NewAttacks
{
    public class EnemyAttackSystem : MonoBehaviour
    {
        public bool MasterRequest { get; private set; } = false;
        public bool Request { get; private set; } = false;

        public bool SetMasterRequest(bool set) => MasterRequest = set;
        public bool SetRequest(bool set) => Request = set;
    }
}
