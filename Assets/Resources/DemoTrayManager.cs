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

        //Update the last node with the same ID in the Dict = newNode
        DictLastNodeByTileID[iDTile] = newNode;

        UpdateTileInTray();

        HandleMatch3(PickedTile.IDTile);
    }

    private void UpdateTileInTray()
    {
        int index = 0;

        foreach (DemoTileController tile in TrayTiles)
        {
            tile.transform.position = TrayPositions[index].position;
            index++;
        }
    }

    private void HandleMatch3(int newTileID)
    {
        var tilesWithSameID = TrayTiles.Where(x => x.IDTile == newTileID).ToList();

        if (tilesWithSameID.Count == 3)
        {
            foreach (DemoTileController tile in tilesWithSameID)
            {
                //Remove 3 tiles with the same ID in the LinkedList and set them inactive
                TrayTiles.Remove(tile);
                tile.gameObject.SetActive(false);
            }

            //Remove the entry with key = newTileID in the Dict, because there is no tile with that ID in the LinkedList after removing 3 tiles
            DictLastNodeByTileID.Remove(newTileID);

            UpdateTileInTray();
        }
        else if (TrayTiles.Count >= 7)
        {
            Debug.Log("Không ghép được, khay đã đầy!");
        }
    }
}
