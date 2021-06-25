using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StratSceneUI : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button quiteBtn;

    private void Awake()
    {       
        startBtn.onClick.AddListener(OnClickStartBtn);
        quiteBtn.onClick.AddListener(OnClickQuiteBtn);
    }
    private void OnClickStartBtn()
    {
        SceneManager.LoadScene("MainScene");
    }
    private void OnClickQuiteBtn()
    {
        Application.Quit();
    }
}