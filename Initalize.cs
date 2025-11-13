using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Operation {
    public int idx, val, op;
    // 0: ChangeValue
    // 1: OpenDoor
    // 2: CloseDoor
    // 3: Delay
    // 4: AddValue
    // 5: MoveText

}

public class PositionText {
    public Vector3 StartPoint;
    public Vector3 EndPoint;
    public string value;
}

public class Initalize : MonoBehaviour
{
    public GameObject[] Boxes;
    public DoorAnimation[] Door;
    public NewTextModify[] Label;
    public Spawner_Code MoveText;
    public Canvas OutputCanvas;
    public string UpdateOutputValue;
    public GameObject AllBoxes, ParentMoveText, RobotText, RealRobot;
    public RobotAnimation RobotA;
    public int IndexChange = -1, ActionChange = -1, TimeStop = 0, Accumulator = 0;
    public bool ActionExisted = false, ActionMoving = false, TextMoving = false, UpdateOutput = false, RobotRotating = false, GonnaRest = false, HoldingText = false, GonnaHoldText = false;
    Queue<Operation> AllOperation = new Queue<Operation>();

    public GameObject TextAreaInstruction, TextAreaInput, CompileButton;
    public TextMeshProUGUI Instruction, Input, AnotherDisplayAccumulator;
    Queue<int> AllInput = new Queue<int>();
    public int Output;

    private bool GonnaCompile = true;
    private int FakeAccumulator = 0;
    private int[] FakeBoxes = new int[100];

    // Start is called before the first frame update
    void Start()
    {
        // public string s = DisplayAccumulator.GetComponent<TextMeshProUGUI>();
        OutputCanvas.enabled = false;
        RobotA = RealRobot.GetComponent<RobotAnimation>();
        MoveText = ParentMoveText.gameObject.transform.GetChild(0).gameObject.GetComponent<Spawner_Code>();
        for(int i = 0; i < 100; ++i) {
            Boxes[i] = AllBoxes.gameObject.transform.GetChild(i).gameObject;
            Label[i] = Boxes[i].GetComponent<NewTextModify>();
            Door[i] = Boxes[i].gameObject.transform.GetChild(0).GetChild(0).GetComponent<DoorAnimation>();
            Label[i].Init(i);
        }
    }

    public Operation Cope(int idx, int val, int op) {
        var Op = new Operation();
        Op.idx = idx; Op.val = val; Op.op = op;
        return Op;
    }

    public PositionText Chill(float StartX, float StartY, float StartZ, float EndX, float EndY, float EndZ, string value) {
        var PT = new PositionText();
        PT.StartPoint = new Vector3(StartX, StartY, StartZ);
        PT.EndPoint = new Vector3(EndX, EndY, EndZ);
        PT.value = value;
        return PT;
    }

    // Put value from Box into Accumulator
    void Op5(int idx) {
        AllOperation.Enqueue(Cope(idx, 0, 11));
        AllOperation.Enqueue(Cope(idx, 0, 1));

        AllOperation.Enqueue(Cope(idx, FakeBoxes[idx], -1));
        AllOperation.Enqueue(Cope(100, 0, 12));
        AllOperation.Enqueue(Cope(100, FakeBoxes[idx], -2));

        AllOperation.Enqueue(Cope(100, FakeBoxes[idx], 0));
        AllOperation.Enqueue(Cope(idx, 0, 2));
        AllOperation.Enqueue(Cope(0, 0, 13));
        FakeAccumulator = FakeBoxes[idx];
    }

    // Put value from Accumulator into Box
    void Op3(int idx) {
        // Code to animate number to fly up from Accumulator
        AllOperation.Enqueue(Cope(100, 0, 11));
        AllOperation.Enqueue(Cope(100, FakeAccumulator, -1));
        
        AllOperation.Enqueue(Cope(idx, 0, 12));
        AllOperation.Enqueue(Cope(idx, 0, 1));
        AllOperation.Enqueue(Cope(idx, FakeAccumulator, -2));

        AllOperation.Enqueue(Cope(idx, FakeAccumulator, 0));
        AllOperation.Enqueue(Cope(idx, 0, 2));
        AllOperation.Enqueue(Cope(0, 0, 13));
        FakeBoxes[idx] = FakeAccumulator;
    }

    // Get value from Box and add into Accumulator
    void Op1(int idx) {
        AllOperation.Enqueue(Cope(idx, 0, 11));
        AllOperation.Enqueue(Cope(idx, 0, 1));

        AllOperation.Enqueue(Cope(idx, FakeBoxes[idx], -1));
        AllOperation.Enqueue(Cope(100, 0, 12));
        AllOperation.Enqueue(Cope(100, FakeBoxes[idx], -2));

        AllOperation.Enqueue(Cope(100, FakeBoxes[idx], 4));
        AllOperation.Enqueue(Cope(idx, 0, 2));
        AllOperation.Enqueue(Cope(0, 0, 13));
        FakeAccumulator += FakeBoxes[idx];
    }

    // Get value from Box and subtract from Accumulator
    void Op2(int idx) {
        AllOperation.Enqueue(Cope(idx, 0, 11));
        AllOperation.Enqueue(Cope(idx, 0, 1));

        AllOperation.Enqueue(Cope(idx, FakeBoxes[idx], -1));
        AllOperation.Enqueue(Cope(100, 0, 12));
        AllOperation.Enqueue(Cope(100, FakeBoxes[idx], -2));

        AllOperation.Enqueue(Cope(100, -FakeBoxes[idx], 4));
        AllOperation.Enqueue(Cope(idx, 0, 2));
        AllOperation.Enqueue(Cope(0, 0, 13));
        FakeAccumulator -= FakeBoxes[idx];
    }

    void Op901() {
        // Code to summon number from Input Bracket
        int value = AllInput.Peek();
        
        AllOperation.Enqueue(Cope(102, value, -3));
        AllOperation.Enqueue(Cope(102, 0, 11));
        AllOperation.Enqueue(Cope(102, value, -1));
        AllOperation.Enqueue(Cope(100, 0, 12));
        AllOperation.Enqueue(Cope(100, value, -2));

        AllOperation.Enqueue(Cope(100, value, 0));
        AllOperation.Enqueue(Cope(0, 0, 13));
        FakeAccumulator = value; AllInput.Dequeue();
    }

    void Op902() {
        // Code to summon number from Accumulator
        AllOperation.Enqueue(Cope(100, 0, 11));
        AllOperation.Enqueue(Cope(100, FakeAccumulator, -1));
        AllOperation.Enqueue(Cope(101, 0, 12));
        AllOperation.Enqueue(Cope(101, FakeAccumulator, -2));

        AllOperation.Enqueue(Cope(101, FakeAccumulator, 0));
        AllOperation.Enqueue(Cope(0, 0, 13));
        // Output = FakeAccumulator;
    }

    // Just update value of a box from nowhere
    void UpdateValue(int idx, int value) {
        AllOperation.Enqueue(Cope(idx, 0, 1)); 
        AllOperation.Enqueue(Cope(idx, value, 0));
        AllOperation.Enqueue(Cope(idx, 0, 2));
        FakeBoxes[idx] = value;
    }

    void CompileError() {
        
    }

    bool CheckDigit(char c) {
        return (c >= '0' && c <= '9');
    }

    public void Compile() {
        if(!GonnaCompile) return;
        GonnaCompile = false;
        string InstructionText = Instruction.text;
        string InputText = Input.text;

        TextAreaInstruction.gameObject.SetActive(false);
        CompileButton.gameObject.SetActive(false);
        TextAreaInput.gameObject.SetActive(false);
        OutputCanvas.enabled = true;

        int InputLength = InputText.Length;
        for(int i = 0; i < InputLength; ++i) {
            int idx = i, value = 0;
            while(idx < InputLength && CheckDigit(InputText[idx])) {
                value = value * 10 + (InputText[idx] - '0');
                ++idx;
            }
            if(i != idx) {
                i = idx - 1;
                AllInput.Enqueue(value);
            }
        }

        int[] EncodeOperation = new int[100];
        int InstructionLength = InstructionText.Length, Counter = 0;
        for(int i = 0; i < InstructionLength - 2; i += 4) {
            while(i < InstructionLength && !CheckDigit(InstructionText[i]))
                ++i;
            EncodeOperation[Counter] = Convert.ToInt32(InstructionText.Substring(i, 3));
            Counter += 1;
        }

        int ptr = 0;
        while(true) {
            int OpType = EncodeOperation[ptr] / 100;
            int OpValue = EncodeOperation[ptr] % 100;
            if(OpType == 0) {
                break;
            }
            else if(OpType == 9) {
                if(OpValue == 1) Op901();
                if(OpValue == 2) Op902();
            }
            else if(OpType == 1) {
                Op1(OpValue);
            }
            else if(OpType == 2) {
                Op2(OpValue);
            }
            else if(OpType == 3) {
                Op3(OpValue);
            }
            else if(OpType == 5) {
                Op5(OpValue);
            }
            else if(OpType == 6) {
                ptr = OpValue;
                continue;
            }
            else if(OpType == 7 && FakeAccumulator == 0) {
                ptr = OpValue;
                continue;
            }
            else if(OpType == 8 && FakeAccumulator >= 0) {
                ptr = OpValue;
                continue;
            }
            ptr += 1;
        }
    }

    Vector3 GetPositionBox(int idx) {
        if(idx < 100) return new Vector3(12 * (idx % 10) + 6, 12 * (idx / 10) + 6, -12);
        else if(idx == 100) return new Vector3(137f, 49.1f, -40.4f);
        else if(idx == 102) return new Vector3(0.2f, 4.1f, -32.5f);
        else if(idx == 101) return new Vector3(-0.3f, 4.1f, -49.6f);
        return new Vector3(0f, 0f, 0f);
    }

    Vector3 GetRobotHand() {
        Debug.Log(RealRobot.transform.rotation.eulerAngles);
        return RobotText.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Compile();
        if(TimeStop > 0) {
            TimeStop -= 1;
            return;
        }
        if(HoldingText) {
            Vector3 HandPosition = GetRobotHand();
            MoveText.Clone.transform.position = HandPosition;
        }
        if(ActionExisted) {
            if(!Door[IndexChange].DoorIsMoving) {
                if(ActionChange == 1) Door[IndexChange].OpenDoor();
                else Door[IndexChange].CloseDoor();
            }
            if(Door[IndexChange].TimePassed >= 1.0f) {
                ActionExisted = false;
                IndexChange = -1;
                ActionChange = -1;
            }
        }
        else if(TextMoving) {
            TextMoving = MoveText.Moving;
            if(!TextMoving && UpdateOutput) {
                UpdateOutput = false;
                OutputCanvas.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text += UpdateOutputValue;
            }
            if(!TextMoving && GonnaHoldText) {
                GonnaHoldText = false;
                HoldingText = true;
            }
        }
        else if(RobotRotating) {
            if(RobotA.turning == 0) {
                RobotRotating = false;
                if(HoldingText)
                    HoldingText = false;
            }
        }
        else {
            if(AllOperation.Count == 0) return;
            var Current = AllOperation.Peek(); AllOperation.Dequeue();
            if(Current.op == 0) {
                if(Current.idx == 100) {
                    Accumulator = Current.val;
                    AnotherDisplayAccumulator.text = Accumulator.ToString();
                    return;
                }
                else if(Current.idx == 101) {
                    Output = Current.val;
                    return;
                }
                Label[Current.idx].ChangeValue(Current.val);
            }
            else if(Current.op == 4) {
                if(Current.idx == 100) {
                    Accumulator += Current.val;
                    AnotherDisplayAccumulator.text = Accumulator.ToString();
                    return;
                }
            }
            else if(Current.op == 3) {
                TimeStop = Current.val;
            }
            else if(Current.op == -1) {
                int idx = Current.idx;
                Vector3 EndPoint = GetRobotHand();
                string value = Current.val.ToString();
                MoveText.Change(GetPositionBox(idx), EndPoint, value);
                TextMoving = true; GonnaHoldText = true;
            }
            else if(Current.op == -2) {
                int idx = Current.idx;
                Vector3 StartPoint = GetRobotHand();
                string value = Current.val.ToString();
                MoveText.Change(StartPoint, GetPositionBox(idx), value);
                MoveText.OffSoon = true;
                TextMoving = true;
                if(idx == 101) {
                    UpdateOutput = true;
                    UpdateOutputValue = "\n" + value;
                }
            }
            else if(Current.op == -3) {
                int idx = Current.idx;
                string value = Current.val.ToString();
                Vector3 EndPoint = GetPositionBox(102);
                Vector3 StartPoint = new Vector3(EndPoint.x, EndPoint.y + 100, EndPoint.z);
                MoveText.Change(StartPoint, EndPoint, value);
                TextMoving = true;
            }
            else if(Current.op == 11) {
                Vector3 PointingPosition = GetPositionBox(Current.idx);
                RobotA.point1(PointingPosition.x, PointingPosition.z);
                RobotRotating = true;
            }
            else if(Current.op == 12) {
                Vector3 PointingPosition = GetPositionBox(Current.idx);
                RobotA.point2(PointingPosition.x, PointingPosition.z);
                RobotRotating = true;
            }
            else if(Current.op == 13) {
                RobotA.rest(); RobotRotating = true;
            }
            else if(Current.op == 1 || Current.op == 2) {
                ActionExisted = true;
                IndexChange = Current.idx;
                ActionChange = Current.op;
            }
        }
    }
}
