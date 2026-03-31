using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public TextAsset posFile;
    public TextAsset idFile;
    public TextAsset listIDFile;

    public GameObject tilePrefab;

    public Sprite[] allIconSprites;

    private void Start()
    {
        GenerateLevelWithMapping();
    }

    public void GenerateLevelWithMapping()
    {
        if (posFile == null || idFile == null || listIDFile == null)
        {
            Debug.LogError("Don't Reference the file");
            return;
        }

        string[] posLines = posFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] idLines = idFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] listIdLines = listIDFile.text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (posLines.Length != idLines.Length)
        {
            Debug.LogError("Position and ID files must have the same number of lines.");
            return;
        }

        Dictionary<int, string> idToIconMapping = new Dictionary<int, string>();
        int currentListIdIndex = 0;

        for (int i = 0; i < posLines.Length; i++) 
        {
            // Bước 1: Trích xuất tọa độ
            string[] coordinates = posLines[i].Split('-');
            int x = int.Parse(coordinates[0]);
            int y = int.Parse(coordinates[1]);
            int z = int.Parse(coordinates[2]);

            // Bước 2: Trích xuất mã nhóm logic từ tệp ID
            int logicGroupId = int.Parse(idLines[i]);

            // Bước 3: Ánh xạ mã nhóm với một hình ảnh trong tệp ListID
            if (!idToIconMapping.ContainsKey(logicGroupId))
            {
                // Nếu mã nhóm này chưa có hình ảnh, gán cho nó một hình ảnh từ ListID
                // Dùng phép chia lấy dư để lặp lại ListID nếu số lượng mã nhóm nhiều hơn
                string assignedIconName = listIdLines[currentListIdIndex % listIdLines.Length];
                idToIconMapping.Add(logicGroupId, assignedIconName);
                currentListIdIndex++;
            }

            string finalIconName = idToIconMapping[logicGroupId];

            // Bước 4: Khởi tạo khối hình
            float worldX = x;
            float worldY = y;
            float worldZ = -z * 1f;
            Vector3 spawnPosition = new Vector3(worldX, worldY, worldZ);

            GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);

            // Bước 5: Phân lớp hiển thị và gán hình ảnh
            SpriteRenderer bgRenderer = newTile.GetComponent<SpriteRenderer>();
            if (bgRenderer != null)
            {
                bgRenderer.sortingOrder = z;
            }

            SpriteRenderer[] allRenderers = newTile.GetComponentsInChildren<SpriteRenderer>();
            if (allRenderers.Length > 1)
            {
                SpriteRenderer iconRenderer = allRenderers[1];
                iconRenderer.sortingOrder = z + 1;

                // Gọi hàm tìm kiếm hình ảnh tương ứng để gán vào
                iconRenderer.sprite = FindSpriteByName(finalIconName);
            }
        }
    }

    private Sprite FindSpriteByName(string spriteName)
    {
        foreach (Sprite s in allIconSprites)
        {
            if (s.name == spriteName)
            {
                return s;
            }
        }
        return null;
    }
}
