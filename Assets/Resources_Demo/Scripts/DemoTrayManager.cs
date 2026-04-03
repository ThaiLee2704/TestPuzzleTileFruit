using PrimeTween;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoTrayManager : Singleton<DemoTrayManager>
{
    [SerializeField] private Transform[] TrayPositions;
    private LinkedList<DemoTileController> TrayTiles = new LinkedList<DemoTileController>();
    //Dictionary ID Tile mapping to the last Node in the LinkedList with that ID Tile, to optimize adding new tile to the tray
    //Key: TileID - Value: Last node in the LinkedList with that TileID
    private Dictionary<int, LinkedListNode<DemoTileController>> DictLastNodeByTileID = new Dictionary<int, LinkedListNode<DemoTileController>>();

    public static event Action OnLoseGame;
    public static event Action OnMatch3;

    [Button]
    private void Dev()
    {
        foreach (var item in TrayTiles)
        {
            Debug.LogError("tray has: id" + item.IDTile);
        }
        foreach (var item in DictLastNodeByTileID)
        {
            Debug.LogError("DictLastNodeByTileID has: id" + item.Key + " - " + item.Value.Next.Value);
        }
    }

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

        int iDTile = PickedTile.IDTile;

        LinkedListNode<DemoTileController> newNode;

        if (DictLastNodeByTileID.TryGetValue(iDTile, out LinkedListNode<DemoTileController> lastNodeByTileID))
            //If ID of PickedTile same as the ID of Tile in the Dict, add PickedTile after the last Tile with the same ID in the LinkedList
            newNode = TrayTiles.AddAfter(lastNodeByTileID, PickedTile);
        else
            //If ID of PickedTile different from all IDs of Tiles in the Dict, add PickedTile to the end of the LinkedList
            newNode = TrayTiles.AddLast(PickedTile);

        DictLastNodeByTileID[iDTile] = newNode;

        UpdateTileInTray(PickedTile);

        HandleMatch3(iDTile);
    }

    private void UpdateTileInTray(DemoTileController newestTile = null)
    {
        int index = 0;

        foreach (DemoTileController tile in TrayTiles)
        {
            Vector3 endPos = TrayPositions[index].position;

            if (tile == newestTile)
                tile.MoveToTray(endPos, index);//, () => HandleMatch3(newestTile.IDTile));
            else
                tile.MoveInTray(endPos, index);

            index++;
        }
    }

    private List<DemoTileController> temp = new();

    private void HandleMatch3(int newTileID)
    {
        var tilesWithSameID = TrayTiles.Where(x => x.IDTile == newTileID).ToList();

        if (tilesWithSameID.Count == 3)
        {
            for (int i = 0; i < tilesWithSameID.Count; i++)
            {
                var tile = tilesWithSameID[i];
                TrayTiles.Remove(tile);
                Tween.Scale(tile.transform, 2, 0, 0.3f, Ease.InBack).SetDelay(i * 0.05f).OnComplete(() =>
                {
                    tile.gameObject.SetActive(false);
                });
            }

            //Remove the entry with key = newTileID in the Dict, because there is no tile with that ID in the LinkedList after removing 3 tiles
            DictLastNodeByTileID.Remove(newTileID);

            OnMatch3?.Invoke();

            UpdateTileInTray();
        }
        else if (TrayTiles.Count >= 7)
        {
            Debug.Log("Không ghép được, khay đã đầy!");
            OnLoseGame?.Invoke();
        }
    }
    public void OffTile()
    {
        foreach (var item in temp)
        {
            item.gameObject.SetActive(false);
        }
        temp.Clear();
        UpdateTileInTray();
    }
}
