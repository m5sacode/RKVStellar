
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    [Header("Resource Icons")]
    [SerializeField] private Texture2D antimatterIcon;
    [SerializeField] private Texture2D oreIcon;
    [SerializeField] private Texture2D foodIcon;
    [SerializeField] private Texture2D peopleIcon;

    private const float BAR_SIZE = 128f;

    private class ResourceDisplay
    {
        public RadialProgressBar Bar;
        public Label AmountLabel;
        public Label UnitLabel;
    }

    private Dictionary<string, ResourceDisplay> resources =
        new Dictionary<string, ResourceDisplay>();

    private void Start()
    {
        var root = uiDocument.rootVisualElement;

        // Create all four resource bars
        CreateResource(
            root, "AntimatterBar", "Antimatter",
            antimatterIcon, Color.red, 750, 1000
        );

        CreateResource(
            root, "OreBar", "Ore",
            oreIcon, Color.blue, 500, 1000
        );

        CreateResource(
            root, "FoodBar", "Food",
            foodIcon, Color.green, 900, 1000
        );

        CreateResource(
            root, "PeopleBar", "People",
            peopleIcon, Color.yellow, 90, 100
        );
    }

    private void CreateResource(
        VisualElement root,
        string elementName,
        string resourceName,
        Texture2D icon,
        Color color,
        float amount,
        float maximum)
    {
        var container = root.Q<VisualElement>(elementName);

        if (container == null)
        {
            Debug.LogError("Could not find: " + elementName);
            return;
        }

        // Keep each resource inside a fixed square
        //container.style.position = Position.Relative;
        //container.style.width = BAR_SIZE;
        //container.style.height = BAR_SIZE;
        //container.style.minWidth = BAR_SIZE;
        //container.style.minHeight = BAR_SIZE;
        //container.style.maxWidth = BAR_SIZE;
        //container.style.maxHeight = BAR_SIZE;
        //container.style.flexGrow = 0;
        //container.style.flexShrink = 0;

        // Create radial bar
        var bar = new RadialProgressBar();

        bar.ProgressColor = color;
        bar.BackgroundColor = new Color(0.18f, 0.18f, 0.18f);
        bar.Thickness = 8f;

        bar.style.position = Position.Absolute;
        bar.style.left = 0;
        bar.style.top = 0;
        bar.style.width = BAR_SIZE;
        bar.style.height = BAR_SIZE;

        container.Add(bar);

        // Create icon
        var image = new Image();

        image.image = icon;
        image.scaleMode = ScaleMode.ScaleToFit;
        image.pickingMode = PickingMode.Ignore;

        image.style.position = Position.Absolute;
        image.style.width = 26;
        image.style.height = 26;
        image.style.top = 3;
        image.style.left = Length.Percent(50);
        image.style.marginLeft = -13;

        container.Add(image);

        // Create amount label
        var amountLabel = new Label();

        amountLabel.pickingMode = PickingMode.Ignore;

        amountLabel.style.position = Position.Absolute;
        amountLabel.style.top = 45;
        amountLabel.style.left = 0;
        amountLabel.style.right = 0;

        amountLabel.style.fontSize = 15;
        amountLabel.style.color = Color.white;
        amountLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        container.Add(amountLabel);

        // Create resource name
        var unitLabel = new Label(resourceName);

        unitLabel.pickingMode = PickingMode.Ignore;

        unitLabel.style.position = Position.Absolute;
        unitLabel.style.top = 68;
        unitLabel.style.left = 0;
        unitLabel.style.right = 0;

        unitLabel.style.fontSize = 10;
        unitLabel.style.color = new Color(0.7f, 0.7f, 0.7f);
        unitLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

        container.Add(unitLabel);

        // Store references
        var display = new ResourceDisplay
        {
            Bar = bar,
            AmountLabel = amountLabel,
            UnitLabel = unitLabel
        };

        resources[resourceName] = display;

        // Initial values
        SetResource(resourceName, amount, maximum);
    }

    // Update amount and maximum automatically
    public void SetResource(
        string resourceName,
        float amount,
        float maximum)
    {
        if (!resources.TryGetValue(resourceName, out var display))
        {
            Debug.LogWarning("Resource not found: " + resourceName);
            return;
        }

        float percentage = maximum > 0
            ? (amount / maximum) * 100f
            : 0f;

        // Update radial progress
        display.Bar.SetProgress(percentage);

        // Update amount text
        display.AmountLabel.text =
            $"{amount:0.##} / {maximum:0.##}";
    }

    // Optional convenience method for changing just the amount
    public void SetAmount(string resourceName, float amount)
    {
        if (!resources.TryGetValue(resourceName, out var display))
            return;

        // Preserve the current maximum from the displayed text
        string[] values = display.AmountLabel.text.Split('/');

        if (values.Length < 2 ||
            !float.TryParse(values[1].Trim(), out float maximum))
            return;

        SetResource(resourceName, amount, maximum);
    }
}
