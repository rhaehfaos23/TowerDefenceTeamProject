using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum BuildableTowerType
{
    AttackTower         =   1,
    DefenceTower        =   1 << 1,
    WorkerTower         =   1 << 2,
    None                =   0
}

public enum TileType
{
    None                = 0,
    PlayerWalkable      = 1,
    EnemyWalkable       = 1 << 1
}

[System.Serializable]
public class TileInfo
{
    [Tooltip("타일 이름")] public string tileName = null;
    [Tooltip("타일 설명")] public string tileDescription = null;
    [Tooltip("타일위를 걸어다닐수 있는 유닛")] public TileType tileType = TileType.PlayerWalkable;
    [Tooltip("타일위에 설치할수 있는 타워 종류")] public BuildableTowerType buildable = BuildableTowerType.None;
    [Tooltip("둔화율")] [Range(0f, 1f)] public float slowRate = 0f;
    [Tooltip("회피율")] [Range(0f, 1f)] public float dodgeRate = 0f;
    [Tooltip("타일 위에 있을때, 피해를 받으면 해당하는 타입의 데미지는 일정비율 감소되서 받음")] public DamageType decreaseDamageType = DamageType.Melee;
    [Tooltip("감소되는 데미지 퍼센트")] [Range(0f, 1f)] public float decreaseDamageRate = 0f;
    [Tooltip("타일 위에 있을때, 피해를 받으면 해당하는 타입의 데미지는 일정비율 증가되서 받음")] public DamageType increaseDamageType = DamageType.Melee;
    [Tooltip("증가되는 데미지 퍼센트")] [Range(0f, 1f)] public float increaseDamageRate = 0f;
    [Tooltip("도트데미지량")] public int damageOverTime = 0;
    [Tooltip("장애물 타일인가?")] public bool isObstacleTile = false;
    [Tooltip("장애물 타일이면 해체후에 어떤 타일로 바뀌는가?")] public Tile afterBreakTile = null;
    [SerializeField] [Tooltip("타일 고유 번호")] public int id = 0;
    [SerializeField] [Tooltip("타일이 파괴되면 생길 타일")] public TileBase returnTile = null;

    static public TileInfoBuilder Create()
    {
        return new TileInfoBuilder();
    }
}

public enum DamageType
{
    Melee = 1 << 0,
    Fire  = 1 << 1,
    Ice   = 1 << 2,
    Wind  = 1 << 3,
    Wood  = 1 << 4,
    Earth = 1 << 5
}

[RequireComponent(typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    [Header("타일 정보")]
    [SerializeField] TileInfo info = new TileInfo();

    TileBase returnTile;
    Transform ui;
    Tilemap tilemap;
    BoxCollider2D col;
    Bounds bounds;
    bool haveBuilding;
    Vector3 mouseWorldPosition;

    public bool HaveBuilding { get; set; }
    public TileInfo Info { get => info; }
    public int ID { get => info.id; }

    public virtual void Start()
    {
        ui = transform.GetChild(0);
        ui.gameObject.SetActive(false);
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        col = GetComponent<BoxCollider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
        }
        col.size = new Vector2(16f, 16f);
        col.offset = Vector2.zero;
        bounds = col.bounds;
    }

    public void ChangeTile()
    {
        info = info.afterBreakTile.info;
        tilemap.SetTile(tilemap.WorldToCell(mouseWorldPosition), returnTile);
    }

    public void OnDrawGizmos()
    {
        const float radius = 4f;
        switch (info.tileType)
        {
            case TileType.PlayerWalkable:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case TileType.EnemyWalkable:
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case TileType.None:
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
        }
    }
}
