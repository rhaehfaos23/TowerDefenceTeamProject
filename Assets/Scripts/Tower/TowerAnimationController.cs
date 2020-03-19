using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
public class TowerAnimationController : MonoBehaviour
{
    [SpineAnimation] [SerializeField] string idle;
    [SpineAnimation] [SerializeField] string attack;

    SkeletonAnimation skeletonAnimation;
    Spine.AnimationState state;
    string curAnim;
    // Start is called before the first frame update
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        state = skeletonAnimation.state;
    }

    /// <summary>
    /// 타워 애니메이션 실행
    /// idle = 대기 애니메이션
    /// attack = 공격 애니메이션
    /// </summary>
    /// <param name="animaion">애니메이션 이름</param>
    public void PlayAnimation(string animaion)
    {
        switch (animaion)
        {
            case "idle":
                if (curAnim == "idle") break;
                state.SetAnimation(0, idle, true);
                curAnim = "idle";
                break;
            case "attack":
                if (curAnim == "attack") break;
                state.SetAnimation(0, attack, true);
                curAnim = "attack";
                break;
        }
    }
}
