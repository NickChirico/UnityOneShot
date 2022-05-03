using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController _animControl;
    public static AnimationController GetAnimController { get{ return _animControl; } }

    public Animator anim;
    public SpriteRenderer sp;

    private void Awake()
    {
        _animControl = this;

    }

    public void SetWalk(bool b)
    {
        anim.SetBool("isRunning", b);
    }

    public bool GetWalk()
    {
        return anim.GetBool("isRunning");
    }

    public void SetFlipX(Vector2 dir)
    {
        if (dir.x < 0)
            sp.flipX = true;
        else if(dir.x > 0)
            sp.flipX = false;
    }
}
