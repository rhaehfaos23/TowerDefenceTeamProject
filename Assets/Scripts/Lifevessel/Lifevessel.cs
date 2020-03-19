using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LifeMove))]
[RequireComponent(typeof(Rigidbody))]
public class Lifevessel : MonoBehaviour, ITakeDamage
{
    const float multiply = 1024f;
    public event System.Action GameClear;
    public event System.Action GameOver;
    [SerializeField] float maxHP = 0.0f;


    Slider hpBar;
    float currentHP = 0f;
    public float CurrentHP { get => currentHP / multiply; private set => currentHP = value * multiply; }

    private void Awake()
    {
        CurrentHP = maxHP;
    }

    private void Start()
    {
        hpBar = GameObject.Find("LifeVesselHpSlider").GetComponent<Slider>();
        hpBar.value = CurrentHP / maxHP;
    }

    public void TakeDamage(float damage)
    {
        CurrentHP -= damage;
        hpBar.value = CurrentHP / maxHP;
        if (CurrentHP <= 0)
        {
            GameManager.Instance.GameOver = true;
            GameOver?.Invoke();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameEnd")
        {
            Debug.Log("GameClear");
            GameManager.Instance.GameClear = true;
            GameClear?.Invoke();
        }
    }
}
