using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewTextModify : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI Name;
    public int Val = 0;
    private string s;
    void Start()
    {

    }
    private string to_string(int x)
    {
        s = "";
        while(x > 0)
        {
            s = (char)('0' + x % 10) + s;
            x /= 10;
        }
        if (s == "") s = "0";
        return s;
    }
    public void Init(int x)
    {
        if(x < 10) Name.text = '0' + to_string(x);
        else Name.text = to_string(x);
        return;
    }
    public int CurrentVal() {
        return Val;
    }
    public void ChangeValue(int x)
    {
        Val = x;
    }
    void Update()
    {
        
    }
}