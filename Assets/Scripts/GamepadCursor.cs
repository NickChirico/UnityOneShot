using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasTransform;
    [Space(10)]
    [SerializeField] private float trackingSpeed = 1000f;
    
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Camera mainCamera;

    private bool previousMouseState;

    private void OnEnable()
    {
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice(virtualMouse);
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        //playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        /*if(virtualMouse != null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);*/

        InputSystem.onAfterUpdate -= UpdateMotion;
        //playerInput.onControlsChanged -= OnControlsChanged;
    }

    bool doActive = true;
    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
        { return; }

        if (!doActive)
            return;

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= trackingSpeed;

        Vector2 currentPos = virtualMouse.position.ReadValue();
        Vector2 newPos = currentPos + deltaValue;

        newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width);
        newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPos);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool southButtonPressed = Gamepad.current.buttonSouth.IsPressed();
        if (previousMouseState != southButtonPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, southButtonPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = southButtonPressed;
        }

        AnchorCursor(newPos);
    }

    private void AnchorCursor(Vector2 pos)
    {
        Vector2 anchorPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform, pos, canvas.renderMode
            == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchorPos);

        cursorTransform.anchoredPosition = anchorPos;
    }

    private const string gamepadScheme = "XBOX_Controller";
    private const string mouseScheme = "KeyboardMouse";
    private string previousControlScheme = "";
    private void OnControlsChanged(PlayerInput input)
    {
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            SetCursorActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            SetCursorActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(Vector2.zero);
            previousControlScheme = gamepadScheme;
        }
    }

    public void SetCursorActive(bool b)
    {
        doActive = b;

        if (b)
        { 
            cursorTransform.GetComponent<Image>().enabled = true;
            AnchorCursor(Vector2.zero);
            Cursor.visible = false;
        }
        else
        { 
            cursorTransform.GetComponent<Image>().enabled = false;
            Cursor.visible = true;
        }
    }
}
