using UnityEngine;

/// <summary>
/// Enemy‚ÌAI‚ÅBeheviaTree‚Æ•¹—p‚³‚¹‚éƒNƒ‰ƒX
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
