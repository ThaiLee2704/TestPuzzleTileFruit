using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTileController : MonoBehaviour
{
    public int IDTile;
    public int OrderLayer;

    public SpriteRenderer SpriteBG;
    public SpriteRenderer SpriteIcon;
    public BoxCollider2D Collider;

    public List<DemoTileController> UpperTiles = new List<DemoTileController>();
    public List<DemoTileController> LowerTiles = new List<DemoTileController>();

    public static event Action<DemoTileController> OnPickedTile;

    private void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
    }

    public void SetUpTile(int iDTile, int orderLayer, Sprite spriteInDictionary)
    {
        IDTile = iDTile;
        OrderLayer = orderLayer;
        SpriteIcon.sprite = spriteInDictionary;

        SpriteBG.sortingOrder = orderLayer * 10;
        SpriteIcon.sortingOrder = orderLayer * 10 + 1;
    }

    public void CheckStateTile()
    {
        bool haveUpperTile = UpperTiles.Count == 0;

        SpriteIcon.color = haveUpperTile ? Color.white : Color.gray;
        SpriteBG.color = haveUpperTile ? Color.white : Color.gray;
        Collider.enabled = haveUpperTile;
    }

    private void OnMouseDown()
    {
        foreach (DemoTileController lowerTile in LowerTiles)
        {
            lowerTile.RemoveUpperTile(this);
        }

        LowerTiles.Clear();
        Collider.enabled = false;

        OnPickedTile?.Invoke(this);
    }

    public void RemoveUpperTile(DemoTileController upperTile)
    {
        UpperTiles.Remove(upperTile);
        CheckStateTile();
    }
}
