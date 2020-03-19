using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    public enum Type { MASTER, EFFECT}
    [SerializeField] Text text;
    [SerializeField] Type type;

    private void Start()
    {
        StartCoroutine(CO_SyncVolume());
    }

    IEnumerator CO_SyncVolume()
    {
        while (true)
        {
            switch (type)
            {
                case Type.MASTER:
                    if (SoundManager.Instance != null)
                    {
                        int value = Mathf.RoundToInt(SoundManager.Instance.MasterVolume * 10);
                        text.text = value.ToString();
                    }
                    break;

                case Type.EFFECT:
                    if (SoundManager.Instance != null)
                    {
                        int value = Mathf.RoundToInt(SoundManager.Instance.EffectVolume * 10);
                        text.text = value.ToString();
                    }
                    break;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
