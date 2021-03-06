using UnityEngine;

/// <summary>
/// EnemyのAIでBeheviaTreeと併用させるクラス
/// </summary>

namespace NewAttacks
{
    public class EnemyAttackSystem : MonoBehaviour
    {
        public bool MasterRequest { get; private set; } = false;
        public bool Request { get; private set; } = false;
        public bool RunLimitAttack { get; private set; } = false;

        public bool SetMasterRequest(bool set) => MasterRequest = set;
        public bool SetRequest(bool set) => Request = set;
        public bool SetRunLimitAttack(bool set) => RunLimitAttack = set;
    }
}
