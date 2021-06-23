using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraPos;

    private void Update()
    {
        //var pos = Vector3.MoveTowards(transform.position, cameraPos.position, 0.01f);
        var pos = Vector3.Lerp(transform.position, cameraPos.position, 8f*Time.deltaTime);
        var rot = Quaternion.Lerp(transform.rotation, cameraPos.rotation, 4f* Time.deltaTime);
        transform.position = pos;
        transform.rotation = rot;
    }
}
