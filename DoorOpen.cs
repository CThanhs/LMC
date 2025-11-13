using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    public int CurrentAction = 0;
    public int TimeRotation = 0;
    public float AngleRotation = -160.0f, TimePassed = 0.0f;
    public bool DoorIsMoving = false;

    void Start()
    {

    }

    public void OpenDoor() {
        CurrentAction = 1;
    }

    public void CloseDoor() {
        CurrentAction = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentAction == 1) {
            if(TimePassed < 1.0f) {
                float NewTime = Mathf.Min(1.0f - TimePassed, Time.deltaTime);
                float AngleRotate = AngleRotation * NewTime;
                transform.Rotate(0, 0, AngleRotate);
                TimePassed += NewTime;
                DoorIsMoving = true;
            }
            else {
                DoorIsMoving = false;
                CurrentAction = 0;
                TimePassed = 0.0f;
            }
        }
        else if(CurrentAction == 2) {
            if(TimePassed < 1.0f) {
                float NewTime = Mathf.Min(1.0f - TimePassed, Time.deltaTime);
                float AngleRotate = -AngleRotation * NewTime;
                transform.Rotate(0, 0, AngleRotate);
                TimePassed += NewTime;
                DoorIsMoving = true;
            }
            else {
                DoorIsMoving = false;
                CurrentAction = 0;
                TimePassed = 0.0f;
            }
        }
    }
}
