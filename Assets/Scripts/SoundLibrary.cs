using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    [System.Serializable]
    class Music
    {
        public string name;
        public AudioClip clip;
    }

    public static SoundLibrary Instance { get; set; } = null;

    [SerializeField] Music[] musics = null;

    Dictionary<string, AudioClip> musicLibrary = new Dictionary<string, AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            DestroyImmediate(this);
            return;
        }

        for (int i=0; i<musics.Length; ++i)
        {
            musicLibrary.Add(musics[i].name, musics[i].clip);
        }

        DontDestroyOnLoad(this);
    }

    public AudioClip FindMusic(string name)
    {
        if (musicLibrary.ContainsKey(name))
        {
            return musicLibrary[name];
        }
        else
        {
            return null;
        }
    }
}
