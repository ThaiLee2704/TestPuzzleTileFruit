using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct LevelData
{
    public TextAsset posData;
    public TextAsset iDData;
    public TextAsset listIDData;
}

public class DemoLevelManager : MonoBehaviour
{
    [Header("Level Data")]
    public LevelData[] Levels;

    //[Header("Data Level")]
    //[SerializeField] private TextAsset posData;
    //[SerializeField] private TextAsset iDData;
    //[SerializeField] private TextAsset listIDData;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Sprite[] fruitSprites;

    private List<DemoTileController> allTiles = new List<DemoTileController>();

    //private Dictionary<Vector3, DemoTileController> DictPosOfTile = new Dictionary<Vector3, DemoTileController>();

    private void Start()
    {
        //CreatLevel();
        //HandleSortingTile();
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= Levels.Length)
        {
            Debug.LogError("Invalid level index: " + levelIndex);
            return;
        }

        foreach (DemoTileController tile in allTiles)
        {
            Destroy(tile.gameObject);
        }

        allTiles.Clear();
        //DictPosOfTile.Clear();

        LevelData currentLevel = Levels[levelIndex];
        CreatLevel(currentLevel);
        HandleSortingTile();
    }

    void CreatLevel(LevelData levelData)
    {
        //Read data
        string[] posLines = levelData.posData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] iDLines = levelData.iDData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] listIDLines = levelData.listIDData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //Create list ID Icon
        List<int> listIcons = new List<int>();

        foreach (string listIDLine in listIDLines)
        {
            listIcons.Add(int.Parse(listIDLine.Trim()));
        }
        //Random list ID Icon
        listIcons = listIcons.OrderBy(x => Random.value).ToList();

        //Create list ID Tile
        List<int> listIDs = new List<int>();
        foreach (string iDLine in iDLines)
        {
            int iD = int.Parse(iDLine.Trim());
            if (!listIDs.Contains(iD))
            {
                //Add different ID Tile to list ID Tile
                listIDs.Add(iD);
            }
        }

        //Create dictionary to map ID Tile to Icon Sprite
        Dictionary<int, Sprite> DictIconForTile = new Dictionary<int, Sprite>();
        for (int i = 0; i < listIDs.Count; i++)
        {
            int iconID = listIcons[i];
            DictIconForTile.Add(listIDs[i], fruitSprites[iconID]);
        }

        //Create Tile and Set up Tile
        for (int i = 0; i < posLines.Length; i++)
        {
            string[] coordinatesTile = posLines[i].Trim().Split('-');
            float xPos = float.Parse(coordinatesTile[0]);
            float yPos = float.Parse(coordinatesTile[1]);
            int zPos = int.Parse(coordinatesTile[2]);

            int currentTileID = int.Parse(iDLines[i].Trim());

            Vector3 spawnPos = new Vector3(xPos, yPos, 0);
            GameObject newTile = Instantiate(TilePrefab, spawnPos, Quaternion.identity);

            DemoTileController tileController = newTile.GetComponent<DemoTileController>();
            tileController.SetUpTile(currentTileID, zPos, DictIconForTile[currentTileID]);

            allTiles.Add(tileController);

            //DictPosOfTile[spawnPos] = tileController;
        }
    }

    void HandleSortingTile()
    {
        //Find max layer of tile
        int maxLayer = allTiles.Max(tile => tile.OrderLayer);

        foreach (DemoTileController tileA in allTiles)
        {
            //float xTileA = tileA.transform.position.x;
            //float yTileA = tileA.transform.position.y;
            //int zTileA = tileA.OrderLayer;

            ////Find upper tiles of tileA
            //for (int zTileB = zTileA + 1; zTileB <= maxLayer; zTileB++)
            //{
            //    for (int distanceX = -1; distanceX <= 1; distanceX++)
            //    {
            //        for (int distanceY = -1; distanceY <= 1; distanceY++)
            //        {
            //            Vector3 checkPos = new Vector3(xTileA + distanceX, yTileA + distanceY, 0);

            //            if (DictPosOfTile.TryGetValue(checkPos, out DemoTileController tileB))
            //            {
            //                tileA.UpperTiles.Add(tileB);
            //                tileB.LowerTiles.Add(tileA);
            //            }
            //        }
            //    }
            //}

            foreach (DemoTileController tileB in allTiles)
            {
                if (tileA == tileB || tileA.OrderLayer > tileB.OrderLayer) continue;

                float distanceX = Mathf.Abs(tileA.transform.position.x - tileB.transform.position.x);
                float distanceY = Mathf.Abs(tileA.transform.position.y - tileB.transform.position.y);

                if (distanceX < 2f && distanceY < 2f)
                {
                    tileA.UpperTiles.Add(tileB);
                    tileB.LowerTiles.Add(tileA);
                }
            }

            tileA.SetStateTile();
        }
    }
}
