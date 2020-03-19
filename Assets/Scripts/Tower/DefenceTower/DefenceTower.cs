using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceTower : TowerBase, ITakeDamage
{
    public event System.Action OnDead;
    [SerializeField] Image hpBar;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CO_CreatingTower());
    }

    IEnumerator CO_CreatingTower()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitUntil(() => { return creating == false; });
        GetComponent<Collider>().enabled = true;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;

        hpBar.fillAmount = CurrentHP / MaxHP;

        if (CurrentHP <= 0)
        {
            OnDead?.Invoke();
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
