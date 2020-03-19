using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerMoveAnimation : MonoBehaviour
{
    [SerializeField] float shakeForce = 0f;
    [SerializeField] TowerBase parent = null;
    [Tooltip("스프라이트가 보이지 않는 절대값 Z위치")] [SerializeField] float hidableZ = 0f;
    [SerializeField] GameObject magicSquare = null;

    Vector3 oriPos;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        hidableZ = Mathf.Abs(hidableZ);
    }

    public void Shake(float time)
    {
        magicSquare.SetActive(true);
        speed = hidableZ / time;
        oriPos = new Vector3(transform.localPosition.x,
            transform.localPosition.y,
            -hidableZ);
        transform.localPosition = oriPos;
        StartCoroutine(CO_Shake(time));
    }

    IEnumerator CO_Shake(float time)
    {
        while (time > 0 && transform.localPosition.z < 0)
        {
            Vector3 randomPoint = Random.onUnitSphere * shakeForce;
            randomPoint.y = 0;
            transform.localPosition = randomPoint + oriPos;
            oriPos.z += speed * Time.deltaTime;
            time -= Time.deltaTime;

            yield return null;
        }

        transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(0.5f);

        magicSquare.SetActive(false);
    }
}
