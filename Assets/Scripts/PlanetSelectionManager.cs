
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetSelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private InputActionReference selectAction;

    [Header("Raycast")]
    [SerializeField] private float maxDistance = 10000f;

    [Header("Camera Movement")]
    [SerializeField] private float panSmoothTime = 0.8f;

    private PlanetSelectable hoveredPlanet;
    private PlanetSelectable selectedPlanet;

    private Vector3 cameraVelocity;

    private void OnEnable()
    {
        selectAction.action.performed += OnSelect;
        selectAction.action.Enable();
    }

    private void OnDisable()
    {
        selectAction.action.performed -= OnSelect;
        selectAction.action.Disable();

        if (hoveredPlanet != null)
            hoveredPlanet.SetHovered(false);
    }

    private void Update()
    {
        UpdateHover();
        UpdateCameraTarget();
    }

    private void UpdateHover()
    {
        Ray ray = playerCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        PlanetSelectable newHoveredPlanet = null;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            newHoveredPlanet =
                hit.collider.GetComponentInParent<PlanetSelectable>();
        }

        if (newHoveredPlanet == hoveredPlanet)
            return;

        if (hoveredPlanet != null)
            hoveredPlanet.SetHovered(false);

        hoveredPlanet = newHoveredPlanet;

        if (hoveredPlanet != null)
            hoveredPlanet.SetHovered(true);
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        Ray ray = playerCamera.ScreenPointToRay(
            Mouse.current.position.ReadValue()
        );

        PlanetSelectable clickedPlanet = null;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            clickedPlanet =
                hit.collider.GetComponentInParent<PlanetSelectable>();
        }

        if (clickedPlanet == selectedPlanet)
            return;

        if (selectedPlanet != null)
            selectedPlanet.SetSelected(false);

        selectedPlanet = clickedPlanet;

        if (selectedPlanet != null)
            selectedPlanet.SetSelected(true);
    }

    private void UpdateCameraTarget()
    {
        if (selectedPlanet == null)
            return;

        cameraTarget.position = Vector3.SmoothDamp(
            cameraTarget.position,
            selectedPlanet.transform.position,
            ref cameraVelocity,
            panSmoothTime
        );
    }
}
