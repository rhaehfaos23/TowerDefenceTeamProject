using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderAttackConnector : MonoBehaviour
{
    [SerializeField] LeaderAttack leaderAttack;
    void Attack()
    {
        leaderAttack.Attack();
    }
}
