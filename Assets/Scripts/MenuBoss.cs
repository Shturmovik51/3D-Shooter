using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBoss : MonoBehaviour
{
    [SerializeField] private Transform mainPropeller;
    [SerializeField] private Transform backPropeller;
    void Update()
    {
        mainPropeller.Rotate(Vector3.up, 200 * Time.deltaTime);
        backPropeller.Rotate(Vector3.up, 200 * Time.deltaTime);
    }
}
