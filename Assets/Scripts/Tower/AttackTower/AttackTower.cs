using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackTower : TowerBase, IBuffDamage
{
    [Header("공격 타워 공용")]
    [SerializeField] float damage = 0f;
    [Tooltip("몇초에 한번 공격할 것인가?")] [SerializeField] float attackSpeed = 0f;
    [Tooltip("사거리")] [SerializeField] float range = 0f;


    [Header("공격타워 강화")]
    [Tooltip("공격력 레벨당 증가할 공격력(비율, 소수로)")] [SerializeField] float addDamagePerLevel;
    [Tooltip("공격속도 레벨당 감소할 공격속도(비율, 소수로)")] [SerializeField] float minusAttackSpeedPerLevel;
    [Tooltip("사정거리 레벨당 증가할 사정거리량(비율, 소수로)")] [SerializeField] float addRangePerLevel;

    private float realDamage = 0.0f;
    private float realAttackSpeed = 0.0f;
    private float realRange = 0.0f;
    public float MinusAttackSpeedPerLevel { get => minusAttackSpeedPerLevel; }
    public float AddRangerPerLevel { get => addRangePerLevel; }
    public float Damage { get => realDamage * (1f + UpgradeData.AttackLevel * addDamagePerLevel);}
    public float AttackSpeed { get => realAttackSpeed / 256f * (1.0f - UpgradeData.AttackSpeedLevel * minusAttackSpeedPerLevel); set => realAttackSpeed = value * 256f; }
    public float Range { get => realRange / 256f; set => realRange = value * 256f; }
    public float DamageBuffAmount { get; set; } = 0f;
    public void DeleteDamageBuff(float amount)
    {
        realDamage /= amount;
    }

    public void AddDamageBuff(float amount)
    {
        realDamage *= amount;
    }

    protected override void Start()
    {
        base.Start();

        realDamage = damage + DamageBuffAmount;
        Range = range;
    }
}
 