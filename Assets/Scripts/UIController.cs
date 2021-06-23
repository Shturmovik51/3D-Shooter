using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instanse;
    [SerializeField] private Text healthCount;
    [SerializeField] private Text ammoCount;
    [SerializeField] private GameObject winPanel;

    private void Awake()
    {
        instanse = this;
    }
    private void FixedUpdate()
    {
        healthCount.text = Player.instance.PlayerHealth.ThisHealth.ToString();
        ammoCount.text = Player.instance.AmmoCount.ToString();
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

}
