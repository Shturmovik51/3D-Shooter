using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDoor : MonoBehaviour
{
    [SerializeField] private Transform leftD;
    [SerializeField] private Transform rightD;
    [SerializeField] private GameObject elPanel;
    [SerializeField] private Vector3 tar;

    private void Update()
    {
        if (elPanel == null)
        {
            leftD.rotation = Quaternion.Slerp(leftD.transform.rotation, Quaternion.Euler(tar), 1 * Time.deltaTime);
            rightD.rotation = Quaternion.Slerp(rightD.transform.rotation, Quaternion.Euler(-tar), 1 * Time.deltaTime);
        }   
    }
}
