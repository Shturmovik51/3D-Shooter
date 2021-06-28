using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instanse;
    [SerializeField] private Text healthCount;
    [SerializeField] private Text ammoCount;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button winRestBtn;
    [SerializeField] private Button winQuiteBtn;
    [SerializeField] private Button winMainMenuBtn;
    [SerializeField] private Button deathRestBtn;
    [SerializeField] private Button deathQuiteBtn;
    [SerializeField] private Button deathMainMenuBtn;
    [SerializeField] private Button pauseContinueBtn;
    [SerializeField] private Button pauseQuiteBtn;
    [SerializeField] private Button pauseMainMenuBtn;
    [SerializeField] private Text enemyKillsCountTxt;
    [SerializeField] private Text summary;
    public int enemyKillsCount = 0;

    private void Awake()
    {
        instanse = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
            OnEnterPause();

        enemyKillsCountTxt.text = enemyKillsCount.ToString();
    }
    private void FixedUpdate()
    {
        healthCount.text = Player.instance.PlayerHealth.ThisHealth.ToString();
        ammoCount.text = Player.instance.AmmoCount.ToString();
        winQuiteBtn.onClick.AddListener(OnClickQuiteBtn);
        winRestBtn.onClick.AddListener(OnClickRestartBtn);
        winMainMenuBtn.onClick.AddListener(OnClickMainMenyBtn);
        deathQuiteBtn.onClick.AddListener(OnClickQuiteBtn);
        deathRestBtn.onClick.AddListener(OnClickRestartBtn);
        deathMainMenuBtn.onClick.AddListener(OnClickMainMenyBtn);
        pauseContinueBtn.onClick.AddListener(OnExitPause);
        pauseMainMenuBtn.onClick.AddListener(OnClickMainMenyBtn);
        pauseQuiteBtn.onClick.AddListener(OnClickQuiteBtn);
    }

    public void WinGame()
    {
        StartCoroutine(Win());
    }
    private IEnumerator Win()
    {
        yield return new WaitForSeconds(5);
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        StartCoroutine(Lose());
    }
    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        losePanel.SetActive(true);
        if (enemyKillsCount < 4)
            summary.text = $"Вы не оправдали\n наши надежды";
        else if(enemyKillsCount >= 4 && enemyKillsCount < 8)
            summary.text = $"А ты был хорош\n но недостаточно";
        else if (enemyKillsCount >= 8)
            summary.text = $"На родине тебе\n поставят паметник";
    }
    private void OnClickMainMenyBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
    private void OnClickRestartBtn()
    {
       // SceneManager.UnloadSceneAsync("MainScene");
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }
    private void OnClickQuiteBtn()
    {
        Application.Quit();
    }
    private void OnEnterPause()
    {
        Player.instance.isOnPause = true;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    private void OnExitPause()
    {
        pausePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        Player.instance.isOnPause = false;
    }

}
