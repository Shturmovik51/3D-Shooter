using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthCount;
    [SerializeField] private Text ammoCount;

    private void FixedUpdate()
    {
        healthCount.text = Player.instance.PlayerHealth.ThisHealth.ToString();
        ammoCount.text = Player.instance.AmmoCount.ToString();
    }

}
