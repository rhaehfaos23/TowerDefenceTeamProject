using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class LeaderController : MonoBehaviour
{
    cakeslice.Outline outline;
    GameObject hud;
    bool isSelected = false;
    public bool IsSelected {
        get => isSelected;
        set
        {
            isSelected = value;
            outline.enabled = isSelected;
            hud.SetActive(isSelected);
        }
    }
    List<int> movableLayer;
    Skeleton skeleton;
    Color originColor;
    bool isDraging = false;
    bool canMove = false;
    Vector3 prevPos;
    LeaderInfo info;

    private void Awake()
    {
        info = GetComponent<LeaderInfo>();
        outline = info.outline;
        hud = info.hud;
    }

    private void Start()
    {
        SkeletonMecanim mecanim = GetComponentInChildren<SkeletonMecanim>();
        skeleton = mecanim.skeleton;
        originColor = skeleton.GetColor();
        IsSelected = false;
    }


    private void Update()
    {
        if (IsSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveBegin();
            }

            if (isDraging)
            {
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, -1 ^ 1))
                {
                    Move(hit.point);
                }
            }

            if (Input.GetMouseButtonUp(0) && isDraging)
            {
                MoveEnd();
            }
        }
    }

    void MoveBegin()
    {
        GetComponent<Collider>().enabled = false;
        isDraging = true;
        prevPos = transform.position;
        info.IsMoving = true;
    }

    void Move(Vector3 position)
    {
        position.y = 0.05f;
        canMove = true;
        position.y = 0;

        // 이동할 수 있는 곳인지 확인
        RaycastHit[] hits = Physics.RaycastAll(position + Vector3.up, Vector3.down,
            10, 1 << 8 | 1 << 9);
        for (int i=0; i<hits.Length; ++i)
        {
            if (hits[i].collider.gameObject.layer != 8)
            {
                canMove = false;
                break;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(position, 1f);
        for (int i=0; i<colliders.Length && canMove; ++i)
        {
            if (colliders[i].gameObject.layer == 10)
            {
                canMove = false;
            }
        }

        // 이동할 수 있는 곳인지에 따라 리더 색상을 변경해서 표시
        if (canMove)
        {
            SetColorRestore();
        }
        else
        {
            SetRedColor();
        }

        // 이동할 수 있는 곳인지 상관없이 캐릭터 이동
        transform.position = new Vector3(position.x, 0f, position.z);
    }

    void SetRedColor()
    {
        skeleton.SetColor(new Color(1f, 0f, 0f, 0.5f));
    }

    void SetColorRestore()
    {
        skeleton.SetColor(originColor);
    }

    void MoveEnd()
    {
        GetComponent<Collider>().enabled = true;
        IsSelected = false;
        isDraging = false;
        info.IsMoving = false;

        if (canMove)
        {
            // 리더가 움직일수 있는 곳에서 드래그가 끝났을때
        }
        else
        {
            // 리더가 움직일 수 없는 곳에서 드래그가 끝났을때
            transform.position = prevPos; // 원래 위치로 되돌리기
        }
    }
}
