using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTileController : MonoBehaviour
{
    public int maNhomLogic;
    public int chiSoLop;

    [Header("Thành phần hình ảnh")]
    public SpriteRenderer anhNen;
    public SpriteRenderer anhIcon;
    public BoxCollider2D boVaCham;

    public List<DemoTileController> danhSachGachDeLen = new List<DemoTileController>();
    public List<DemoTileController> danhSachGachBiDe = new List<DemoTileController>();

    private void Awake()
    {
        boVaCham = GetComponent<BoxCollider2D>();
    }

    public void ThietLapDuLieu(int maLogic, int lop, Sprite hinhAnh)
    {
        maNhomLogic = maLogic;
        chiSoLop = lop;
        anhIcon.sprite = hinhAnh;

        anhNen.sortingOrder = lop * 10;
        anhIcon.sortingOrder = lop * 10 + 1; 
    }

    public void KiemTraTrangThai()
    {
        if (danhSachGachDeLen.Count == 0)
        {
            anhIcon.color = Color.white;
            anhNen.color = Color.white;

            boVaCham.enabled = true;
        }
        else
        {
            anhIcon.color = Color.gray;
            anhNen.color = Color.gray;

            boVaCham.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        foreach (DemoTileController tile in danhSachGachBiDe)
        {
            tile.GoBoGachDeLen(this);
        }

        this.gameObject.SetActive(false);
    }

    public void GoBoGachDeLen(DemoTileController gachBenTren)
    {
        danhSachGachDeLen.Remove(gachBenTren);
        KiemTraTrangThai();
    }
}
