using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public enum AudioType { Master, Effect }

    public static SoundManager Instance { get; set; } = null;
    [SerializeField] AudioSource master = null;
    [SerializeField] AudioSource effect = null;
    [SerializeField] int BgSoundNumber;

    public float MasterVolume
    {
        get => master.volume;
        set => master.volume = value;
    }

    public float EffectVolume
    {
        get => effect.volume;
        set => master.volume = value;
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(gameObject);

            return;
        }

        StartCoroutine(CO_TitleMusicStart());

        DontDestroyOnLoad(gameObject);
    }

    IEnumerator CO_TitleMusicStart()
    {
        yield return new WaitForEndOfFrame();
        int idx = 0;
        master.loop = false;

        while (true)
        {
            yield return new WaitUntil(() => { return !master.isPlaying; });
            PlayBG("title" + idx.ToString());
            idx = ++idx % BgSoundNumber;
        }
        
    }

    public void MasterVoumeUp()
    {
        master.volume += 0.1f;
    }

    public void MasterVoumeDown()
    {
        master.volume -= 0.1f;
    }

    public void EffectVoumeUp()
    {
        effect.volume += 0.1f;
    }

    public void EffectVoumeDown()
    {
        effect.volume -= 0.1f;
    }

    public void PlayBG(string name)
    {
        master.clip = SoundLibrary.Instance.FindMusic(name);
        master.Play();
    }

    public void PlayEffect(string name)
    {
        effect.PlayOneShot(SoundLibrary.Instance.FindMusic(name));
    }
}
