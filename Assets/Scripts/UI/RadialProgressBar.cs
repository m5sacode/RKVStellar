
using UnityEngine;
using UnityEngine.UIElements;

public class RadialProgressBar : VisualElement
{
    public float Progress { get; set; } = 0f;

    public Color ProgressColor = Color.green;
    public Color BackgroundColor = new Color(0.18f, 0.18f, 0.18f);

    public float Thickness = 8f;

    public RadialProgressBar()
    {
        generateVisualContent += Draw;
    }

    public void SetProgress(float percentage)
    {
        Progress = Mathf.Clamp(percentage, 0f, 100f);
        MarkDirtyRepaint();
    }

    private void Draw(MeshGenerationContext context)
    {
        var painter = context.painter2D;

        float size = Mathf.Min(contentRect.width, contentRect.height);

        Vector2 center = new Vector2(
            contentRect.width / 2f,
            contentRect.height / 2f
        );

        float radius = size / 2f - Thickness;

        if (radius <= 0)
            return;

        // 270 degree arc with a gap at the top
        float startAngle = 225f;
        float sweepAngle = 270f;

        // Background
        DrawArc(
            painter,
            center,
            radius,
            startAngle,
            startAngle - sweepAngle,
            BackgroundColor
        );

        // Progress (counterclockwise)
        if (Progress > 0)
        {
            float progressAngle = sweepAngle * (Progress / 100f);

            DrawArc(
                painter,
                center,
                radius,
                startAngle,
                startAngle - progressAngle,
                ProgressColor
            );
        }
    }

    private void DrawArc(
        Painter2D painter,
        Vector2 center,
        float radius,
        float startAngle,
        float endAngle,
        Color color)
    {
        painter.strokeColor = color;
        painter.lineWidth = Thickness;
        painter.lineCap = LineCap.Round;

        painter.BeginPath();

        int segments = Mathf.Max(
            1,
            Mathf.CeilToInt(Mathf.Abs(endAngle - startAngle) / 2f)
        );

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(
                startAngle,
                endAngle,
                (float)i / segments
            );

            float rad = angle * Mathf.Deg2Rad;

            Vector2 point = center + new Vector2(
                Mathf.Cos(rad) * radius,
                Mathf.Sin(rad) * radius
            );

            if (i == 0)
                painter.MoveTo(point);
            else
                painter.LineTo(point);
        }

        painter.Stroke();
    }
}
