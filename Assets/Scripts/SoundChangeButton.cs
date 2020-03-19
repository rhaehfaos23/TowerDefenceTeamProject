using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundChangeButton : MonoBehaviour
{
    public enum SoundType { Master, Effect}

    [SerializeField] SoundType soundType;

    public void VolumeUp()
    {
        switch (soundType)
        {
            case SoundType.Master:
                SoundManager.Instance.MasterVoumeUp();
                break;
            case SoundType.Effect:
                SoundManager.Instance.EffectVoumeUp();
                break;
        }
    }

    public void VolumeDown()
    {
        switch (soundType)
        {
            case SoundType.Master:
                SoundManager.Instance.MasterVoumeDown();
                break;
            case SoundType.Effect:
                SoundManager.Instance.EffectVoumeDown();
                break;
        }
    }
}
