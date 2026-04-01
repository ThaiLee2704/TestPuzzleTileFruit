using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoTrayManager : Singleton<DemoTrayManager>
{
    public LinkedList<DemoTileController> TrayTiles = new LinkedList<DemoTileController>();
    public Transform[] TrayPositions;

    private void OnEnable()
    {
        DemoTileController.OnPickedTile += AddToTray;
    }

    private void OnDisable()
    {
        DemoTileController.OnPickedTile -= AddToTray;
    }

    public void AddToTray(DemoTileController PickedTile)
    {
        if (TrayTiles.Count >= 7)
            return;

        LinkedListNode<DemoTileController> nutChenSau = null;
        LinkedListNode<DemoTileController> nutHienTai = TrayTiles.First;

        while (nutHienTai != null)
        {
            if (nutHienTai.Value.IDTile == PickedTile.IDTile)
                nutChenSau = nutHienTai;

            nutHienTai = nutHienTai.Next;
        }

        if (nutChenSau != null)
            TrayTiles.AddAfter(nutChenSau, PickedTile);
        else
            TrayTiles.AddLast(PickedTile);

        CapNhatViTriHienThi();

        KiemTraGhepBa(PickedTile.IDTile);
    }

    private void CapNhatViTriHienThi()
    {
        int chiSo = 0;

        foreach (DemoTileController gach in TrayTiles)
        {
            gach.transform.position = TrayPositions[chiSo].position;
            gach.SpriteBG.sortingOrder = 1000 + chiSo * 10;
            gach.SpriteIcon.sortingOrder = 1000 + chiSo * 10 + 1;
            chiSo++;
        }
    }

    private void KiemTraGhepBa(int maLogicVuaThem)
    {
        var gachCungLoai = TrayTiles.Where(x => x.IDTile == maLogicVuaThem).ToList();

        if (gachCungLoai.Count == 3)
        {
            foreach (DemoTileController gach in gachCungLoai)
            {
                TrayTiles.Remove(gach);
                gach.gameObject.SetActive(false);
            }
            CapNhatViTriHienThi();
        }
        else if (TrayTiles.Count >= 7)
        {
            Debug.Log("Không ghép được, khay đã đầy!");
        }
    }
}
