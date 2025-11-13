using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    public GameObject Robot;
    public Animator robotanimation;
    public int turning, pending, type;
    public float angle, curang, lmao, speed=25f;

    float calc(float desire)
    {
        lmao = desire;
        if (Mathf.Abs(desire - curang) <= 180F) return desire - curang;
        else return 360F - (desire - curang);
    }

    public void point1(float x, float z)
    {
        float radians = Mathf.Atan((Robot.transform.position.x - x) / (Robot.transform.position.z - z));
        float angl = calc(radians * (180 / Mathf.PI)) / speed;
        turning = (int)speed;
        pending = (int)1;
        angle = angl;
        type = 0;
    }

    public void point2(float x,float z)
    {
        float radians = Mathf.Atan((Robot.transform.position.x - x) / (Robot.transform.position.z - z));
        float angl = calc(radians * (180 / Mathf.PI)) / speed;
        turning = (int)speed;
        pending = (int)1;
        angle = angl;
        type = 1;
    }

    public void rest()
    {
        float angl = calc(0) / speed;
        turning = (int)speed;
        pending = (int)1;
        angle = angl;
        type = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        robotanimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turning > 0)
        {
            turning--;
            Robot.transform.Rotate(0, angle, 0, Space.Self);
            curang += angle;
        }
        if (pending > 0)
        {
            pending--;
            if (pending == 0)
            {
                if (type == 0) robotanimation.Play("Point1");
                else if (type == 1) robotanimation.Play("Point2");
                else if (type == 2)
                {
                    robotanimation.Play("Rest");
                    pending = 100;
                    type = 3;
                }
                else if (type == 3) robotanimation.Play("Idle");
            }
        }
    }
}
