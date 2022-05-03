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
    public void SetWalkNorth(bool b)
    {
        anim.SetBool("isRunning", b);
    }
    public void SetWalkSouth(bool b)
    {
        anim.SetBool("isRunning", b);
    }

    public void SetRunAnim(int val)
    {
        switch (val)
        {
            case 1: // NORTH
                anim.SetInteger("RunDirection", 1);
                break;
            case 2: // SOUTH
                anim.SetInteger("RunDirection", 2);
                break;
            case 3: // SIDE
                anim.SetInteger("RunDirection", 3);
                break;
            default: // 
                anim.SetInteger("RunDirection", 0);
                break;
        }
    }

    public int GetRunAnim()
    {
        return anim.GetInteger("RunDirection");
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

    // New Stuff Below !

    public void SetMoveDirection(Vector2 dir, bool doRun)
    {
        anim.SetFloat("Horizontal", dir.x);
        anim.SetFloat("Vertical", dir.y);
        anim.SetBool("DoRun", doRun);
    }

    bool isDead = false;
    public void TakeDamageAnim()
    {
        if (!isDead)
        {
            anim.SetTrigger("TakeDamage");
        }
    }

    public void DieAnim()
    {
        anim.SetTrigger("Die");
        isDead = true;
    }
}
