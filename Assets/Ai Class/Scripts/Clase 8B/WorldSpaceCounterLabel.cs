using UnityEngine;
using UnityEngine.UI;

public static class WorldSpaceCounterLabel
{
    public static Text Create(Vector3 worldPosition)
    {
        GameObject canvasObject = new GameObject("CounterCanvas", typeof(Canvas));
        canvasObject.transform.position = worldPosition;
        canvasObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        canvasObject.transform.localScale = Vector3.one * 0.01f;

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObject.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 60f);

        GameObject textObject = new GameObject("Label", typeof(Text));
        textObject.transform.SetParent(canvasObject.transform, false);

        Text label = textObject.GetComponent<Text>();
        label.text = "0";
        label.alignment = TextAnchor.MiddleCenter;
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.fontSize = 40;
        label.color = Color.white;

        RectTransform textRect = label.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return label;
    }
}
