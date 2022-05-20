using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController _pauseControl;
    public static PauseController GetPauseController { get { return _pauseControl; } }

    private PlayerInputActions inputActions;

    /*MovementController moveControl;
    MeleeController meleeControl;
    ShotController shotControl;
    AltShotController altControl;*/

    private void Awake()
    {
        _pauseControl = this;

        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();
    }
    private void Start()
    {
        /*moveControl = MovementController.GetMoveController;
        meleeControl = MeleeController.GetMeleeControl;
        shotControl = ShotController.GetShotControl;
        altControl = AltShotController.GetAltControl;*/

        // DONT NEED - add "pause" checks in PlayerState base to account for everything
    }

    public void TogglePause(bool doPause)
    {
        if(doPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}
