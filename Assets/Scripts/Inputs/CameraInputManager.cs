
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAround;
    [SerializeField] private InputActionReference zoom;

    private void OnEnable()
    {
        zoom.action.started += OnZoomStarted;
        zoom.action.canceled += OnZoomCanceled;
    }

    private void OnDisable()
    {
        zoom.action.started -= OnZoomStarted;
        zoom.action.canceled -= OnZoomCanceled;

        lookAround.action.Enable();
    }

    private void OnZoomStarted(InputAction.CallbackContext context)
    {
        lookAround.action.Disable();
    }

    private void OnZoomCanceled(InputAction.CallbackContext context)
    {
        lookAround.action.Enable();
    }
}
