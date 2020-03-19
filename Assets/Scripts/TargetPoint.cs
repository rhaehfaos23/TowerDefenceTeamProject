using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class TargetPoint : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, 0.1f, 0));
        if (plane.Raycast(ray, out float enter))
        {
            transform.position = ray.GetPoint(enter);
        }
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
    }
}
