using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HashTableBucketView : MonoBehaviour
{
    public Image backgroudImage;
    public TMP_Text indexText;
    public TMP_Text contentText;

    private int bucketIndex;

    private static readonly Color EmptyColor = Color.grey;
    private static readonly Color OccupiedColor = Color.green;
    private static readonly Color DeletedColor = Color.orange;

    public void SetIndex(int index)
    {
        bucketIndex = index;
        indexText.text = $"I: {bucketIndex}";
    }

    public void SetEmpty()
    {
        backgroudImage.color = EmptyColor;
        contentText.text = string.Empty;
    }

    public void SetOccupied(string key, string value)
    {
        backgroudImage.color = OccupiedColor;
        contentText.text = $"K: {key}\t\tV:{value}";
    }

    public void SetDeleted()
    {
        backgroudImage.color = DeletedColor;
        contentText.text = "(deleted)";
    }

    public void SetChainedItesm(List<string> items)
    {
        if (items == null || items.Count == 0)
        {
            SetEmpty();
            return;
        }

        backgroudImage.color = OccupiedColor;
        contentText.text = string.Join(" -> ", items);
    }
}
