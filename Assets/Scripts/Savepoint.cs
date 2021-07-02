using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savepoint : MonoBehaviour
{
    [SerializeField] private Player playerSample;
    [SerializeField] private GameObject cameraSample;
    public bool isActive;

    private void Start()
    {
        GameManager.instance.savepoints.Add(this);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            GameManager.instance.ResetSavePoints();
            isActive = true;
        }
    }

    public void Ressurect()
    {        
        Destroy(Player.instance.listener);
        Destroy(Player.instance);

        var player = Instantiate(playerSample);
        var cam = Instantiate(cameraSample);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }
}
