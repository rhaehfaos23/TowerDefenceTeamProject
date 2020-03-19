using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class Miner : TowerBase
{
    #region Inspector
    [Header("광부 속성")]
    [Tooltip("데미지")]
    [SerializeField] float damage = 0;
    [Tooltip("작업 속도 (몇초에 한번씩 캘 것인가)")]
    [SerializeField] float workSpeed = 0f;
    #endregion

    public float Damage { get => damage; }
    public float WorkSpeed { get => workSpeed; }

    protected override IEnumerator CreatedTower()
    {
        Spine.Unity.SkeletonMecanim skeletonMecanim = GetComponentInChildren<Spine.Unity.SkeletonMecanim>();
        Spine.Skeleton skeleton = skeletonMecanim.skeleton;
        Color originColor = skeleton.GetColor();
        while (creating)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000.0f, 1 << 12))
            {
                skeleton.SetColor(originColor);
                creatable = true;
            }
            else
            {
                skeleton.SetColor(new Color(1f, 0f, 0f, 0.5f));
                creatable = false;
            }

            if (Physics.Raycast(mouseRay, out RaycastHit hit2))
            {
                transform.position = hit2.point;
            }

            yield return null;
        }
    }

    public override void CreateComplite()
    {
        if (creatable)
        {
            creating = false;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 100000, 1 << 12))
            {
                Obstacle obstacle = hit.collider.transform.GetComponent<Obstacle>();

                bool isWorkable = obstacle.StartMine(Damage, WorkSpeed, gameObject);
                if (!isWorkable)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
