using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Spawner_Code : MonoBehaviour
{
    public Canvas Canva;
    public GameObject Clone;
    public TextMeshProUGUI Number;
    public float Speed = 20;
    public Vector3 StartPoint, EndPoint;
    public bool Moving = false, OffSoon = false;
    public void Start()
    {
        Speed = 20;
        Clone.transform.position = StartPoint;
        Canva.enabled = false;
    }
    public void Change(Vector3 StartInput, Vector3 EndInput, string value)
    {
        Moving = true;
        Number.SetText(value);
        Clone.transform.position = StartInput;
        StartPoint = StartInput;
        EndPoint = EndInput;
    }
    void Update()
    {
        if (Moving)
        {
            Canva.enabled = true;
            if (Clone.transform.position != EndPoint)
            {
                Clone.transform.position = Vector3.MoveTowards(Clone.transform.position, EndPoint, Speed * Time.deltaTime);
            }
            else
            {
                Moving = false;
                if(OffSoon) {
                    OffSoon = false;
                    Canva.enabled = false;
                }
            }
        }
    }
}
