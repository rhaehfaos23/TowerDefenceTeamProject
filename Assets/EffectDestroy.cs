using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class EffectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<SkeletonAnimation>();
        animation.AnimationState.Complete += (track) => { Destroy(gameObject); };
    }
}
