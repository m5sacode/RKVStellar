
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlanetSelectable : MonoBehaviour
{
    [Header("Ring Settings")]
    [SerializeField] private Material ringMaterial;
    [SerializeField] private float ringRadius = 1.5f;
    [SerializeField] private float ringWidth = 0.025f;
    [SerializeField] private int segments = 80;

    [Header("Transparency")]
    [SerializeField, Range(0f, 1f)]
    private float hoverOpacity = 0.3f;

    private LineRenderer ring;
    private Camera mainCamera;

    public bool IsSelected { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;

        GameObject ringObject = new GameObject("SelectionRing");
        ringObject.transform.SetParent(transform, false);

        ring = ringObject.AddComponent<LineRenderer>();

        ring.useWorldSpace = true;
        ring.loop = true;
        ring.positionCount = segments;
        ring.startWidth = ringWidth;
        ring.endWidth = ringWidth;
        ring.sharedMaterial = ringMaterial;

        ring.enabled = false;
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (ring.enabled && mainCamera != null)
            UpdateRingPosition();
    }

    private void UpdateRingPosition()
    {
        Vector3 center = transform.position;
        Vector3 right = mainCamera.transform.right;
        Vector3 up = mainCamera.transform.up;

        // Slightly offset toward camera to avoid visual overlap.
        center += mainCamera.transform.forward * -0.02f;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            Vector3 point = center +
                right * Mathf.Cos(angle) * ringRadius +
                up * Mathf.Sin(angle) * ringRadius;

            ring.SetPosition(i, point);
        }
    }

    public void SetHovered(bool hovered)
    {
        UpdateVisual(hovered, IsSelected);
    }

    public void SetSelected(bool selected)
    {
        IsSelected = selected;
        UpdateVisual(false, selected);
    }


    private void UpdateVisual(bool hovered, bool selected)
    {
        bool visible = hovered || selected;

        ring.enabled = visible;

        if (!visible)
            return;

        float opacity = selected ? 1f : hoverOpacity;

        Color color = Color.cyan;
        color.a = opacity;

        ring.startColor = color;
        ring.endColor = color;
    }

}
