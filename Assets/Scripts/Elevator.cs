using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int elevatorSpeed;
    [SerializeField] private int openDoorSpeed;
    [SerializeField] private GameObject ElPanelLeft;
    [SerializeField] private GameObject ElPanelRight;
    [SerializeField] private Transform leftD;
    [SerializeField] private Transform rightD;
    public bool isPressedButton;
    public bool isNeedOpen;
    public bool isNeedClose;
    public bool isAlarm;

    private void Start()
    {
        isNeedOpen = true;
        isAlarm = true;
    }

    void FixedUpdate()
    {
        if (isPressedButton)
        {
            ElevatorUp();
        }

        if (ElPanelRight == null)
        {
            isAlarm = false;
        }

        if (isNeedOpen && isAlarm == false && ElPanelLeft == null)
        {
            OpenDoors();
        }

        if(isNeedClose == true)
        {
            CloseDoors();
        }
    }

    private void ElevatorUp()
    {
        if (transform.position.y < 160)
        {
            transform.Translate(Vector3.up * elevatorSpeed * Time.fixedDeltaTime);
            player.transform.parent = transform;
        }
        else
        {
            isPressedButton = false;
            isNeedOpen = true;
            player.transform.parent = null;
        }
    }

    private void OpenDoors()
    {
        if (rightD.transform.position.x < 14)
        {
            leftD.Translate(Vector3.left * openDoorSpeed * Time.fixedDeltaTime);
            rightD.Translate(Vector3.right * openDoorSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isNeedOpen = false;
        }       
    }
    private void CloseDoors()
    {
        if (rightD.transform.position.x > 5)
        {
            leftD.Translate(Vector3.right * openDoorSpeed * Time.fixedDeltaTime);
            rightD.Translate(Vector3.left * openDoorSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isNeedClose = false;
            isPressedButton = true;
        }
    }





}
