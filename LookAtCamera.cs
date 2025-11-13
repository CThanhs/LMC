using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LookAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Ok;
    void Start()
    {
        Ok.transform.LookAt(Camera.main.transform);
        Ok.transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Ok.transform.LookAt(Camera.main.transform);
        Ok.transform.Rotate(0, 180, 0);
    }
}
