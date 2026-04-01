using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoTrayManager : Singleton<DemoTrayManager>
{
    public LinkedList<DemoTileController> danhSachTrongKhay = new LinkedList<DemoTileController>();
    public Transform[] viTriKhay;

    private void OnEnable()
    {
        DemoTileController.SuKienGachDuocChon += ThemVaoKhay;
    }

    private void OnDisable()
    {
        DemoTileController.SuKienGachDuocChon -= ThemVaoKhay;
    }

    public void ThemVaoKhay(DemoTileController gachDuocChon)
    {
        if (danhSachTrongKhay.Count >= 7)
        {
            //Debug.Log("Khay đã đầy, không thể thêm gạch nữa!");
            return;
        }

        LinkedListNode<DemoTileController> nutChenSau = null;
        LinkedListNode<DemoTileController> nutHienTai = danhSachTrongKhay.First;

        while (nutHienTai != null)
        {
            if (nutHienTai.Value.iDTile == gachDuocChon.iDTile)
                nutChenSau = nutHienTai;

            nutHienTai = nutHienTai.Next;
        }

        if (nutChenSau != null)
            danhSachTrongKhay.AddAfter(nutChenSau, gachDuocChon);
        else
            danhSachTrongKhay.AddLast(gachDuocChon);

        CapNhatViTriHienThi();

        KiemTraGhepBa(gachDuocChon.iDTile);
    }

    private void CapNhatViTriHienThi()
    {
        int chiSo = 0;

        foreach (DemoTileController gach in danhSachTrongKhay)
        {
            gach.transform.position = viTriKhay[chiSo].position;
            gach.anhNen.sortingOrder = 1000 + chiSo * 10;
            gach.anhIcon.sortingOrder = 1000 + chiSo * 10 + 1;
            chiSo++;
        }
    }

    private void KiemTraGhepBa(int maLogicVuaThem)
    {
        var gachCungLoai = danhSachTrongKhay.Where(x => x.iDTile == maLogicVuaThem).ToList();

        if (gachCungLoai.Count == 3)
        {
            foreach (DemoTileController gach in gachCungLoai)
            {
                danhSachTrongKhay.Remove(gach);
                gach.gameObject.SetActive(false);
            }
            CapNhatViTriHienThi();
        }
        else if (danhSachTrongKhay.Count >= 7)
        {
            Debug.Log("Không ghép được, khay đã đầy!");
        }
    }
}
