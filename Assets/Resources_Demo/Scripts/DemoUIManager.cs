using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoUIManager : MonoBehaviour
{
    [SerializeField] Image LosePopup;
    [SerializeField] Button replay;

    private void Awake()
    {
        replay.onClick.AddListener(OnReplayBtnClicked);
    }

    private void OnReplayBtnClicked()
    {
        //HideLosePopup();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnEnable()
    {
        DemoTrayManager.OnLoseGame += ShowLosePopup;
    }

    private void OnDisable()
    {
        DemoTrayManager.OnLoseGame -= ShowLosePopup;
    }

    private void ShowLosePopup()
    {
        LosePopup.gameObject.SetActive(true);
    }

    private void HideLosePopup()
    {
        LosePopup.gameObject.SetActive(false);
    }
    //[SerializeField] Button LevelBtn;
    //[SerializeField] Image LevelPopup;

    //private void Awake()
    //{
    //    LevelBtn.onClick.AddListener(OnLevelBtnClicked);
    //}

    //private void OnLevelBtnClicked()
    //{
    //    if (LevelPopup.gameObject.activeSelf)
    //    {
    //        LevelPopup.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        LevelPopup.gameObject.SetActive(true);
    //    }
    //}

    //[SerializeField] private Button[] LevelButtons;

    //private void Awake()
    //{
    //    for (int i = 0; i < LevelButtons.Length; i++)
    //    {
    //        int levelIndex = i; // Capture the current index for the lambda
    //        LevelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
    //    }
    //}
}
