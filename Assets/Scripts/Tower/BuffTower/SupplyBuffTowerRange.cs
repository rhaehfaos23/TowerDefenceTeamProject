using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBuffTowerRange : BuffTowerRange
{
    [SerializeField] SupplierProjectile projectile;
    [SerializeField] Animator animator;
    List<TowerBase> towers;

    protected override void Start()
    {
        base.Start();
        towers = new List<TowerBase>();
        StartCoroutine(Buff());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Tower") return;
        TowerBase tb = other.GetComponent<TowerBase>();
        towers.Add(tb);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tower")
        {
            TowerBase tb = other.GetComponent<TowerBase>();
            if (towers.Contains(tb))
            {
                towers.Remove(tb);
            }
        }
    }

    IEnumerator Buff()
    {
        while (true)
        {
            if (towers.Count > 0)
            {
                towers.Sort(new TowerComaprer());
                bool isBuff = false;
                for (int i=0; i<towers.Count; ++i)
                {
                    TowerBase tb = towers[i];
                    if (tb.IsCreating || tb.IsDraging || tb.IsMoving) continue;
                    tb.CurrentSupply += (int)tower.Amount;
                    var created = Instantiate(projectile, transform.position, Quaternion.identity);
                    created.StartPos = transform.position;
                    created.EndPos = towers[0].transform.position;
                    isBuff = true;
                    break;

                }

                if (isBuff) yield return new WaitForSeconds(tower.BuffSpeed);
                else yield return new WaitForSeconds(0.05f);
            }

            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}

public class TowerComaprer : IComparer<TowerBase>
{
    public int Compare(TowerBase x, TowerBase y)
    {
        return x.CurrentSupply - y.CurrentSupply;
    }
}
