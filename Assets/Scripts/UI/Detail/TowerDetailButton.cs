using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerDetailButton : MonoBehaviour
{
    
    public int TowerIdx { get; set; } = -1;
    

    Image img;
    Text towerName;
    Text towerDesc;
    TowerBase tb;

    IEnumerator Start()
    {
        yield return new WaitWhile(() => { return TowerIdx == -1; });

        img = GetComponent<Image>();
        tb = TowerManager.inst.GetTower(TowerIdx);
        Sprite sprites = Resources.Load<Sprite>(tb.ID);
        img.sprite = sprites;

        GetComponent<Button>().onClick.AddListener(ShowTowerDetail);
        var btn = GetComponent<Image>();
        btn.type = Image.Type.Simple;
        btn.preserveAspect = true;
    }

    void ShowTowerDetail()
    {
        TowerDetailManager.Instance.TargetChange(tb.TowerName, TowerIdx);
    }
}
