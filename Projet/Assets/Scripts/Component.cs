using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Component : MonoBehaviour
{
    public enum Type {Buffer, NOT, AND, OR, XOR, NAND, NOR, XNOR, Custom, Unset};
    public List<IO> ios = new List<IO>();
    public int inputs;
    public int outputs;
    public SpriteRenderer shapeRenderer;
    public string boolExpression;
    public string compName;
    public Color color = Color.cyan;
    public int[] truthTable;
    public bool truthTableDone = false;
    public Type type = Type.Unset;
    public bool held;
    public Vector2 heldPoint;
    public BoxCollider2D compCollider;

    public Component(ComponentData data)
    {
        type = (Type)data.type;
        inputs = data.inputs;
        outputs = data.outputs;
        truthTable = data.truthTable;
    }
    private void Start()
    {
        SetCompGraphics((int)type);
        SetCorrectGateType((int)type);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UpdateCompGraphics();
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SetCorrectGateType((int)type + 1);
            UpdateCompGraphics();
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            SetCorrectGateType((int)type - 1);
            UpdateCompGraphics();
        }
    }
    public void Move(Vector2 pos, bool snapToGrid)
    {
        if(snapToGrid)
        {
            transform.position = new Vector2(RoundToNearest((pos - heldPoint).x, SelectionManager.singleton.gridSize), RoundToNearest((pos - heldPoint).y, SelectionManager.singleton.gridSize));
        }
        else
        {
            transform.position = pos - heldPoint; 
        }

        AppManager.singleton.UpdateWires();

        SetGateProperties();
    }
    public Vector2 CountInputsOutputs()
    {
        int inputCount = 0;
        int outputCount = 0;
        foreach(IO i in ios)
        {
            if(i.input) inputCount++; else outputCount++;
        }
        return new Vector2(inputCount, outputCount);
    }
    float RoundToNearest(float n, float x) 
    {
        return Mathf.Round(n / x) * x;
    }

    public void UpdateState()
    {
        if(type == Type.Unset)
            return;      
    }

    public void UpdateCompGraphics()
    {
        SetCompGraphics((int)type);
    } 
    public void SetCompGraphics(int index)
    {
        if(type != Type.Custom && type != Type.Unset)
        {
            switch(ComponentsGraphicManager.singleton.standard)
            {
                case ComponentsGraphicManager.LogicGatesStandard.ANSI:
                    shapeRenderer.sprite = ComponentsGraphicManager.singleton.spritesANSI[index%8];
                break;
                case ComponentsGraphicManager.LogicGatesStandard.IEC: 
                    shapeRenderer.sprite = ComponentsGraphicManager.singleton.spritesIEC[index%8];
                break;
                case ComponentsGraphicManager.LogicGatesStandard.DIN:
                    shapeRenderer.sprite = ComponentsGraphicManager.singleton.spritesDIN[index%8];
                break;
            }
        }
        else
        {
            shapeRenderer.sprite = ComponentsGraphicManager.singleton.defaultComponent;
            shapeRenderer.color = color;
        }
    }

    public void SetCorrectGateType(int index)
    {
        switch(index%10)
        {
            case 0: type = Type.Buffer; break;
            case 1: type = Type.NOT; break;
            case 2: type = Type.AND; break;
            case 3: type = Type.OR; break;
            case 4: type = Type.XOR; break;
            case 5: type = Type.NAND; break;
            case 6: type = Type.NOR; break;
            case 7: type = Type.XNOR; break;
            case 8: type = Type.Custom; break;
            case 9: type = Type.Unset; break;
        }

        foreach(IO i in ios)
        {
            i.FlushIO();
        }
        ios.Clear();

        if(type == Type.Custom)
        {
            for(int i = 0; i < inputs; i++)
            {
                ios.Add(new IO(true, false, Vector2.zero, new List<IO>(), this));
            }
            for(int i = 0; i < outputs; i++)
            {
                ios.Add(new IO(false, false, Vector2.zero, new List<IO>(), this));
            }
        }
        else
        {
            if(type == Type.Buffer || type == Type.NOT)
            {
                ios.Add(new IO(true, false, Vector2.zero, new List<IO>(), this));
                ios.Add(new IO(false, false, Vector2.zero, new List<IO>(), this));
            }
            else
            {
                ios.Add(new IO(true, false, Vector2.zero, new List<IO>(), this));
                ios.Add(new IO(true, false, Vector2.zero, new List<IO>(), this));
                ios.Add(new IO(false, false, Vector2.zero, new List<IO>(), this));
            }   
        }
        
        SetGateProperties();
    }

    public void SetGateProperties()
    {
        Vector2 S = shapeRenderer.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        compCollider = GetComponent<BoxCollider2D>();

        Vector2 inputsOutputs = CountInputsOutputs();

        int inIndex = 0;
        int outIndex = 0;

        foreach(IO i in ios)
        {
            if(i.input)
            {
               i.pos = new Vector2(transform.position.x - compCollider.bounds.size.x/2, transform.position.y - compCollider.bounds.size.y/2 + (compCollider.bounds.size.y/(inputsOutputs.x + 1))*(inIndex+1));
               inIndex++;
            }
            else
            {
                i.pos = new Vector2(transform.position.x + compCollider.bounds.size.x/2, transform.position.y - compCollider.bounds.size.y/2 + (compCollider.bounds.size.y/(inputsOutputs.y + 1))*(outIndex+1));
                outIndex++;
            }    
        }
        
        if(compName == "")
        {
            compName = type.ToString();
        }
    }

    public void ComputeOutputStates()
    {
        List<IO> inputs = new List<IO>();
        List<IO> outputs = new List<IO>();
        foreach(IO i in ios)
        {
            if(i.input)
            {
                inputs.Add(i);
            }
            else
            {
                outputs.Add(i);
            }
        }

        if(type != Type.Custom)
        {
            switch(type)
            {
                case Type.Buffer : outputs[0].state = inputs[0].state; break;
                case Type.NOT : outputs[0].state = !inputs[0].state; break;
                case Type.AND : outputs[0].state = inputs[0].state && inputs[1].state; break;
                case Type.OR : outputs[0].state = inputs[0].state || inputs[1].state; break;
                case Type.XOR : outputs[0].state = inputs[0].state != inputs[1].state ? true : false; break;
                case Type.NAND : outputs[0].state = !(inputs[0].state && inputs[1].state); break;
                case Type.NOR : outputs[0].state = !(inputs[0].state || inputs[1].state); break;
                case Type.XNOR : outputs[0].state = inputs[0].state == inputs[1].state ? true : false; break;
            }
        }
        else
        {
            string inputString = "";

            for(int i = 0; i < inputs.Count; i++)
            {
                inputString += inputs[i].state == true ? 1 : 0;
            }

            int index = Convert.ToInt32(inputString, 2);

            for(int i = 0; i < outputs.Count; i++)
            {
                outputs[i].state = truthTable[(index * outputs.Count) + i] == 1 ? true : false;
            }   
        }
    }

    public void FlushComponent()
    {
        List<Wire> toDestroy = new List<Wire>();
        foreach(Wire w in AppManager.singleton.wires)
        {
            if(w.start.component == this || w.end.component == this)
            {
                toDestroy.Add(w);
            }
        }
        WireTool.singleton.DestroyWires(toDestroy.ToArray());
    }
}
