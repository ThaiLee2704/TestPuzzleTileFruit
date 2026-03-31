using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public int zLayer;
    public int logicGroupId;
    public bool isCovered = false;

    private SpriteRenderer[] renderers;
    private Vector2 checkSize = new Vector2(0.9f, 0.9f);

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void CheckOverlap()
    {
        isCovered = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, checkSize, 0f);

        foreach (Collider2D col in colliders)
        {
            TileController otherTile = col.GetComponent<TileController>();

            if (otherTile != null && otherTile != this)
            {
                if (otherTile.zLayer > this.zLayer)
                {
                    isCovered = true;
                    break;
                }
            }
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        Color targetColor = isCovered ? Color.gray : Color.white;

        foreach (SpriteRenderer sr in renderers)
        {
            sr.color = targetColor;
        }
    }

    void OnMouseDown()
    {
        if (!isCovered)
        {
            Debug.Log("Đã chọn khối có mã logic: " + logicGroupId);

        }
    }
}
