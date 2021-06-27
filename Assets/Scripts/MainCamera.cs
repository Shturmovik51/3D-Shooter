using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;
    [SerializeField] private Transform playerHead;

    private void Update()
    {
        if (Player.instance.isDead == false)
        {
            var pos = Vector3.Lerp(transform.position, cameraPos.position, 8f * Time.deltaTime);
            var rot = Quaternion.Lerp(transform.rotation, cameraPos.rotation, 4f * Time.deltaTime);
            transform.position = pos;
            transform.rotation = rot;
        }
        else
        {
            var deathPos = playerHead.position - transform.position;
            var deathDir = Vector3.RotateTowards(transform.forward, deathPos, 8f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(deathDir);
        }


    }
}
