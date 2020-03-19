using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderAttackSkill : LeaderSkill
{
    [Header("공격스킬 속성")]
    [Tooltip("공격력")] [SerializeField] float damage = 0f;
}
