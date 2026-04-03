using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using PrimeTween;

public class DemoTileController : MonoBehaviour
{
    [Header("Anim Tile to Tray")]
    [SerializeField] private AnimationCurve CurveAnimTileToTray;

    private int iDTile;
    public int IDTile => iDTile;
    private int orderLayer;
    public int OrderLayer => orderLayer;

    [SerializeField] private SpriteRenderer spriteBG;
    [SerializeField] private SpriteRenderer spriteIcon;
    [SerializeField] private BoxCollider2D colli;

    public List<DemoTileController> UpperTiles = new List<DemoTileController>();
    public List<DemoTileController> LowerTiles = new List<DemoTileController>();

    public static event Action<DemoTileController> OnPickedTile;

    private void Awake()
    {
        colli = GetComponent<BoxCollider2D>();
    }

    public void SetUpTile(int iDTile, int orderLayer, Sprite spriteInDictionary)
    {
        this.iDTile = iDTile;
        this.orderLayer = orderLayer;
        spriteIcon.sprite = spriteInDictionary;

        spriteBG.sortingOrder = orderLayer * 10;
        spriteIcon.sortingOrder = orderLayer * 10 + 1;
    }

    public void SetStateTile()
    {
        bool haveUpperTile = UpperTiles.Count == 0;

        spriteIcon.color = haveUpperTile ? Color.white : Color.gray;
        spriteBG.color = haveUpperTile ? Color.white : Color.gray;
        colli.enabled = haveUpperTile;
    }

    //BUG LOGIC: ĐANG MOVE TILE TO TRAY, HANDLE MATCH 3, THÌ KHÔNG CHO PICK TILE KHÁC.
    private void OnMouseDown()
    {
        foreach (DemoTileController lowerTile in LowerTiles)
        {
            lowerTile.RemoveUpperTile(this);
        }

        LowerTiles.Clear();
        colli.enabled = false;

        OnPickedTile?.Invoke(this);
    }

    public void RemoveUpperTile(DemoTileController upperTile)
    {
        UpperTiles.Remove(upperTile);
        SetStateTile();
    }

    public void MoveToTray(Vector3 endPos, int trayIndex, Action OnAfterMoveToTray = null)
    {
        spriteBG.sortingOrder = 2000 + trayIndex * 10;
        spriteIcon.sortingOrder = 2000 + trayIndex * 10 + 1;

        float duration = 0.4f;
        float jumpPower = 2.0f;

        float peakY = Mathf.Max(transform.position.y, endPos.y) + jumpPower;

        Vector3 scaleChuan = Vector3.one * 2f;

        Tween.PositionX(transform, endPos.x, duration, Ease.Linear);

        Sequence.Create()
            .Chain(Tween.PositionY(transform, peakY, duration / 2f, Ease.OutQuad))
            .Chain(Tween.PositionY(transform, endPos.y, duration / 2f, Ease.InQuad))

            .Chain(Tween.Scale(transform, scaleChuan * 0.8f, 0.08f, Ease.OutQuad))
            .Chain(Tween.Scale(transform, scaleChuan * 1.15f, 0.1f, Ease.InOutSine))
            .Chain(Tween.Scale(transform, scaleChuan * 0.95f, 0.08f, Ease.InOutSine))
            .Chain(Tween.Scale(transform, scaleChuan * 1.05f, 0.08f, Ease.InOutSine))
            .Chain(Tween.Scale(transform, scaleChuan, 0.05f, Ease.OutQuad))
            .OnComplete(() =>
            {
                OnAfterMoveToTray?.Invoke();
            });
    }

    public void MoveInTray(Vector3 endPos, int trayIndex)
    {
        spriteBG.sortingOrder = 1000 + trayIndex * 10;
        spriteIcon.sortingOrder = 1000 + trayIndex * 10 + 1;
        //Vector3.();

        Tween.Position(transform, endPos, 0.2f, Ease.OutQuad).SetDelay(.2f);
    }
}
