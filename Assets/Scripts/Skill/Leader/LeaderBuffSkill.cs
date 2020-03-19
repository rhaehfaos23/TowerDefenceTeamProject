using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBuffSkill : LeaderSkill
{
    [Header("버프스킬 속성")]
    [Tooltip("버프 수치 (1 + 소수로)")] [SerializeField] float percent = 0;
}
