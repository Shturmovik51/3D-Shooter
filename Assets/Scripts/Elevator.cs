using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //[SerializeField] private Player player;
    [SerializeField] private int elevatorSpeed;
    [SerializeField] private int openDoorSpeed;
    [SerializeField] private ElectricalPanel ElPanelLeft;
    [SerializeField] private ElectricalPanel ElPanelRight;
    [SerializeField] private Transform leftD;
    [SerializeField] private Transform rightD;
    [SerializeField] private Boss boss;
    public bool isPressedButton;
    public bool isDoorClosed;
    public bool isUp;

    private void Start()
    {
        isDoorClosed = true;
    }

    void FixedUpdate()
    {
        if (isUp)
        {
            ElevatorUp();
        }

        if (isDoorClosed && !isUp && ElPanelRight.isDestroyed == true && ElPanelLeft.isDestroyed == true)
        {
            OpenDoors();
        }

        if(!isDoorClosed && isPressedButton)
        {
            CloseDoors();
        }
    }
    private void ElevatorUp()
    {        
        if (transform.position.y < 160)
        {
            transform.Translate(Vector3.up * elevatorSpeed * Time.fixedDeltaTime);
            //player.transform.parent = transform;
        }
        else
        {
            isPressedButton = false;
            isDoorClosed = true;
           // player.transform.parent = null;
            isUp = false;
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
            isDoorClosed = false;
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
            isDoorClosed = true;
            isPressedButton = true;
            isUp = true;
            boss.FirstActivateBoss();
        }
    }





}
