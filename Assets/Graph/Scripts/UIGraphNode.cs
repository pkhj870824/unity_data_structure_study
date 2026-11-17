using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIGraphNode : MonoBehaviour
{
    public Image image;
    public TMP_Text text;

    private GraphNode node;

    public void Reset()
    {
        if (node == null)
        {
            return;
        }

        SetColor(node.Canvisit ? Color.white : Color.gray);
        SetText($"ID: {node.id}\nWeight: {node.weight}");
    }

    public void SetNode(GraphNode node)
    {
        this.node = node;
        Reset();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void SetText(string value)
    {
        text.text = value;
    }
}