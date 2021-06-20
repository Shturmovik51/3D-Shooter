using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    [SerializeField] private Transform leftD;
    [SerializeField] private Transform rightD;
    [SerializeField] private ElectricalPanel elPanel;
    [SerializeField] private Vector3 tar;

    private void Update()
    {
        if (elPanel.isDestroyed == true)
        {
            leftD.rotation = Quaternion.Slerp(leftD.transform.rotation, Quaternion.Euler(tar), 1 * Time.deltaTime);
            rightD.rotation = Quaternion.Slerp(rightD.transform.rotation, Quaternion.Euler(-tar), 1 * Time.deltaTime);
        }   
    }
}
