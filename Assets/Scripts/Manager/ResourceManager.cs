using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance { get; private set; } = null;
    [SerializeField] int startGold = 0;
    [Tooltip("기본 골드 증가량")] [SerializeField] int baseGoldStep = 0;
    [SerializeField] int[] goldSteps = null;
    [SerializeField] Text goldText = null;
    [Tooltip("몇초 단위?")][SerializeField] int delta = 10;
    const string goldFormat = "Can : ";
    int gold;

    public int Gold { get => gold ^ -1; private set => gold = value ^ -1; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);
        }

        Gold = startGold;
        StartCoroutine(CO_GetGold());
    }

    /// <summary>
    /// 골드 증가 코루틴
    /// 게임이 종료 될때까지
    /// </summary>
    IEnumerator CO_GetGold()
    {
        int index = 0;
        int second = 0;
        while (true)
        {
            second = ++second % delta;
            if (second == 0)
            {
                index++;
            }

            if (index >= goldSteps.Length)
            {
                index = goldSteps.Length - 1;
            }
            IncreaseGold(baseGoldStep + goldSteps[index]);
            yield return new WaitForSeconds(1f - Time.deltaTime);
        }
    }
    
    // 골드 증가
    public void IncreaseGold(int g)
    {
        Gold += g;

        goldText.text = goldFormat + Gold.ToString("D6");
    }

    public void DecreaseGold(int g)
    {
        Debug.Log("Previous Gold : " + Gold + ",  Gold Decrease : " + g.ToString());
        Gold -= g;
        Debug.Log("Current Gold : " + Gold);

        if (Gold < 0)
        {
            Gold = 0;
        }

        goldText.text = goldFormat + Gold.ToString("D6");
    }
}
