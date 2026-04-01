using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoLevelManager : MonoBehaviour
{
    public TextAsset tepPos;
    public TextAsset tepID;
    public TextAsset tepListID;

    public GameObject banMauVienGach;
    public Sprite[] mangHinhAnhTraiCay;

    private List<DemoTileController> danhSachTatCaGach = new List<DemoTileController>();

    private Dictionary<Vector3, DemoTileController> banDoKhongGian = new Dictionary<Vector3, DemoTileController>();

    private void Start()
    {
        TaoManChoi();
        TinhToanMangLuoiCheLapToiUu();
    }

    void TaoManChoi()
    {
        string[] dongPos = tepPos.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] IDLines = tepID.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        string[] dongListID = tepListID.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        List<int> danhSachHinhAnh = new List<int>();

        foreach (string dong in dongListID)
        {
            danhSachHinhAnh.Add(int.Parse(dong.Trim()));
        }

        danhSachHinhAnh = danhSachHinhAnh.OrderBy(x => Random.value).ToList();

        List<int> danhSachNhomLogic = new List<int>();
        foreach (string dong in IDLines)
        {
            int maLogic = int.Parse(dong.Trim());
            if (!danhSachNhomLogic.Contains(maLogic))
            {
                danhSachNhomLogic.Add(maLogic);
            }
        }

        Dictionary<int, Sprite> tuDienAnhXa = new Dictionary<int, Sprite>();
        for (int i = 0; i < danhSachNhomLogic.Count; i++)
        {
            int maHinhAnh = danhSachHinhAnh[i];
            tuDienAnhXa.Add(danhSachNhomLogic[i], mangHinhAnhTraiCay[maHinhAnh]);
        }

        for (int i = 0; i < dongPos.Length; i++)
        {
            string[] toaDo = dongPos[i].Trim().Split('-');
            float trucX = float.Parse(toaDo[0]);
            float trucY = float.Parse(toaDo[1]);
            int trucZ = int.Parse(toaDo[2]);

            int currentTileID = int.Parse(IDLines[i].Trim());

            Vector3 viTriSinh = new Vector3(trucX, trucY, -trucZ);
            GameObject vienGachMoi = Instantiate(banMauVienGach, viTriSinh, Quaternion.identity);

            DemoTileController trinhDieuKhienTile = vienGachMoi.GetComponent<DemoTileController>();
            trinhDieuKhienTile.ThietLapDuLieu(currentTileID, trucZ, tuDienAnhXa[currentTileID]);

            danhSachTatCaGach.Add(trinhDieuKhienTile);

            banDoKhongGian[viTriSinh] = trinhDieuKhienTile;
        }
    }

    void TinhToanMangLuoiCheLap()
    {
        for (int i = 0; i < danhSachTatCaGach.Count; i++)
        {
            DemoTileController gachA = danhSachTatCaGach[i];

            for (int j = 0; j < danhSachTatCaGach.Count; j++)
            {
                if (i == j) continue;

                DemoTileController gachB = danhSachTatCaGach[j];

                if (gachA.chiSoLop > gachB.chiSoLop)
                {
                    float khoangCachX = Mathf.Abs(gachA.transform.position.x - gachB.transform.position.x);
                    float khoangCachY = Mathf.Abs(gachA.transform.position.y - gachB.transform.position.y);

                    if (khoangCachX < 2f && khoangCachY < 2f)
                    {
                        gachA.danhSachGachBiDe.Add(gachB);
                        gachB.danhSachGachDeLen.Add(gachA);
                    }
                }
            }
        }

        foreach (DemoTileController gach in danhSachTatCaGach)
        {
            gach.KiemTraTrangThai();
        }
    }

    void TinhToanMangLuoiCheLapToiUu()
    {
        foreach (DemoTileController gachA in danhSachTatCaGach)
        {
            float x = gachA.transform.position.x;
            float y = gachA.transform.position.y;
            int z = gachA.chiSoLop;

            foreach (DemoTileController gachB in danhSachTatCaGach)
            {
                if (gachA == gachB || gachA.chiSoLop > gachB.chiSoLop) continue;

                float khoangCachX = Mathf.Abs(x - gachB.transform.position.x);
                float khoangCachY = Mathf.Abs(y - gachB.transform.position.y);

                if (khoangCachX < 2f && khoangCachY < 2f)
                {
                    gachA.danhSachGachDeLen.Add(gachB);
                    gachB.danhSachGachBiDe.Add(gachA);
                }
            }

            gachA.KiemTraTrangThai();
        }
    }
}
