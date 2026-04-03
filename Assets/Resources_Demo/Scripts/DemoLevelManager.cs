using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.ParticleSystem;

//[System.Serializable]
//public struct LevelData
//{
//    public TextAsset posData;
//    public TextAsset iDData;
//    public TextAsset listIDData;
//}

public class DemoLevelManager : MonoBehaviour
{
    [Header("Level Data")]
    //public LevelData[] Levels;

    [Header("Data Level")]
    [SerializeField] private TextAsset posData;
    [SerializeField] private TextAsset listIDData;
    [SerializeField] private TextAsset iDData;

    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Sprite[] fruitSprites;

    [SerializeField] private GameObject particleClicked;

    private List<DemoTileController> allTiles = new List<DemoTileController>();

    public static event Action OnClicked; 

    //private Dictionary<Vector3, DemoTileController> DictPosOfTile = new Dictionary<Vector3, DemoTileController>();

    private void Start()
    {
        CreatLevel();
        HandleSortingTile();
    }

    //public void LoadLevel(int levelIndex)
    //{
    //    if (levelIndex < 0 || levelIndex >= Levels.Length)
    //    {
    //        Debug.LogError("Invalid level index: " + levelIndex);
    //        return;
    //    }

    //    foreach (DemoTileController tile in allTiles)
    //    {
    //        Destroy(tile.gameObject);
    //    }

    //    allTiles.Clear();
    //    //DictPosOfTile.Clear();

    //    LevelData currentLevel = Levels[levelIndex];
    //    CreatLevel(currentLevel);
    //    HandleSortingTile();
    //}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
            GameObject particle = ObjectPooling.Instant.GetObject(particleClicked, this.transform);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            particle.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
            particle.SetActive(true);
            StartCoroutine(waitDisableParticle(particle, .5f));
        }
    }

    IEnumerator waitDisableParticle(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    void CreatLevel(/*LevelData levelData*/)
    {
        //Read data
        string[] posLines = /*levelData.*/posData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] iDLines = /*levelData.*/iDData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] listIDLines = /*levelData.*/listIDData.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        //Create list ID Icon
        List<int> listIcons = new List<int>();

        foreach (string listIDLine in listIDLines)
        {
            listIcons.Add(int.Parse(listIDLine.Trim()));
        }
        //Random list ID Icon
        listIcons = listIcons.OrderBy(x => UnityEngine.Random.value).ToList();

        //Create list ID Tile
        List<int> listIDTiles = new List<int>();
        foreach (string iDLine in iDLines)
        {
            int iD = int.Parse(iDLine.Trim());
            if (!listIDTiles.Contains(iD))
            {
                //Add different ID Tile to list ID Tile
                listIDTiles.Add(iD);
            }
        }

        //Create dictionary to map ID Tile to Icon Sprite
        Dictionary<int, Sprite> DictIconForTile = new Dictionary<int, Sprite>();
        if (listIcons.Count > fruitSprites.Length)
        {
            Debug.LogError("The ListID data > Array Sprite");
            return;
        }
        for (int i = 0; i < listIDTiles.Count; i++)
        {
            int iconID = listIcons[i];
            DictIconForTile.Add(listIDTiles[i], fruitSprites[iconID]);
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
            newTile.name = "Tile " + i;

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
                if (tileA == tileB || tileA.OrderLayer >= tileB.OrderLayer) continue;

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
