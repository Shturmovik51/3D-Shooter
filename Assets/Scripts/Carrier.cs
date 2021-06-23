using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
    private bool isWithCargo = false;
    public List<Transform> colTransforms;

    private void Start()
    {
        colTransforms = new List<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (colTransforms.Count == 0)
                return;
            TakeEntity();
        }
    }
    private void OnTriggerEnter(Collider col)    
    {
        if (col.gameObject.CompareTag("Barrel"))
        {
            colTransforms.Add(col.transform);
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Barrel"))
        {
            colTransforms.Remove(col.transform);
        }
    }

    private void TakeEntity()
    {
        if (!isWithCargo)
        {
            colTransforms[0].parent = transform;
            colTransforms[0].position = transform.position;
        }

        if (isWithCargo)
        {
            colTransforms[0].parent = null;
            colTransforms.Clear();
        }

        isWithCargo = (isWithCargo == true) ? false : true;

    }
}
