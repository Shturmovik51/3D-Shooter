using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Elevator : MonoBehaviour
{
   
    [SerializeField] private int elevatorSpeed;
    public bool isPressedButton;
    
    void FixedUpdate()
    {
        if (isPressedButton)
        {
            ElevatorUp();
        }
    }

    private void ElevatorUp()
    {
        if (transform.position.y < 160)
        {
            transform.Translate(Vector3.up * elevatorSpeed * Time.fixedDeltaTime);
        }
        else isPressedButton = false;
    }
        



}