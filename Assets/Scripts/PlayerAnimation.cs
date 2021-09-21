using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] staticDirections = { "StaticN", "StaticNW", "StaticW", "StaticSW", "StaticS", "StaticSE", "StaticE", "StaticNE" };
    public string[] runDirections = { "RunN", "RunNW", "RunW", "RunSW", "RunS", "RunSE", "RunE", "RunNE" };

    int lastDirection;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
    }

    public void SetDirection(Vector2 dir)
    {
        string[] directionArray = null;

        if (dir.magnitude < 0.01)
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;
            lastDirection = DirectionToIndex(dir);
        }

        // ANIMATOR PLAY ANIMATION
        // 
        //
        //anim.Play(directionArray[lastDirection]);
    }

    // CONVERTS Vector2 direction to an index of a sliced circle
    private int DirectionToIndex(Vector2 dir)
    {
        Vector2 normDir = dir.normalized;

        float step = 360 / 8; // One Circle and 8 Slices
        float offset = step / 2;

        float angle = Vector2.SignedAngle(Vector2.up, normDir);

        angle = angle + offset;
        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }


}
