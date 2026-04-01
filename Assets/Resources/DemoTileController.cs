using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTileController : MonoBehaviour
{
    public int iDTile;
    public int chiSoLop;

    [Header("Thành phần hình ảnh")]
    public SpriteRenderer anhNen;
    public SpriteRenderer anhIcon;
    public BoxCollider2D boVaCham;

    public List<DemoTileController> danhSachGachDeLen = new List<DemoTileController>();
    public List<DemoTileController> danhSachGachBiDe = new List<DemoTileController>();

    public static event Action<DemoTileController> SuKienGachDuocChon;

    private void Awake()
    {
        boVaCham = GetComponent<BoxCollider2D>();
    }

    public void ThietLapDuLieu(int iDTile, int lop, Sprite hinhAnh)
    {
        this.iDTile = iDTile;
        chiSoLop = lop;
        anhIcon.sprite = hinhAnh;

        anhNen.sortingOrder = lop * 10;
        anhIcon.sortingOrder = lop * 10 + 1;
    }

    public void KiemTraTrangThai()
    {
        bool khongBiDe = danhSachGachDeLen.Count == 0;

        anhIcon.color = khongBiDe ? Color.white : Color.gray;
        anhNen.color = khongBiDe ? Color.white : Color.gray;
        boVaCham.enabled = khongBiDe;
    }

    private void OnMouseDown()
    {
        foreach (DemoTileController tile in danhSachGachBiDe)
        {
            tile.GoBoGachDeLen(this);
        }

        danhSachGachBiDe.Clear();
        boVaCham.enabled = false;

        SuKienGachDuocChon?.Invoke(this);
    }

    public void GoBoGachDeLen(DemoTileController gachBenTren)
    {
        danhSachGachDeLen.Remove(gachBenTren);
        KiemTraTrangThai();
    }
}
