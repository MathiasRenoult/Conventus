using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
/// <summary>
/// This class is one of the biggest class of the project. A component is an element which can be placed on the canvas
/// and linked to other components. There's mainly two types of components : the gates and the custom. If a component
/// has a "gate" type, the way the outputs are computed is much faster because its hardcoded. 
/// </summary>
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
    public ulong[] truthTable;
    public bool truthTableDone = false;
    public Type type = Type.Unset;
    public bool held;
    public Vector2 heldPoint;
    public BoxCollider2D compCollider;
    /// <summary>
    /// This constructor takes a "ComponentData" class as input and converts it to a regular "Component" class.
    /// </summary>
    /// <param name="data">The data used as model</param>
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
    /// <summary>
    /// Moves the component to the wanted position. Usually the cursor position.
    /// </summary>
    /// <param name="pos">Position to move to</param>
    /// <param name="snapToGrid">Should the component snap to the grid ?</param>
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
    /// <summary>
    /// Analyses all the IOs of the component and returns the number of inputs and outputs as a "Vector2".
    /// </summary>
    /// <returns>Vector2 with x being the number of inputs and y the number of outputs</returns>
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
    /// <summary>
    /// Simple function rounding n to nearest multiple of x.
    /// </summary>
    /// <param name="n">Number to round</param>
    /// <param name="x">Multiple</param>
    /// <returns>Float being the rounded at multiple of n to x</returns>
    float RoundToNearest(float n, float x) 
    {
        return Mathf.Round(n / x) * x;
    }
    /// <summary>
    /// Shortcut to the function "SetCompGraphics".
    /// </summary>
    public void UpdateCompGraphics()
    {
        SetCompGraphics((int)type);
    } 
    /// <summary>
    /// Takes an index and sets the component to the corresponding type according to the active gate norm. 
    /// Updates the color of the component too.
    /// </summary>
    /// <param name="index">Index representing the type we want to set the component to</param>
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
            shapeRenderer.color = GetColorFromType(type);
        }
        else
        {
            shapeRenderer.sprite = ComponentsGraphicManager.singleton.defaultComponent;
            shapeRenderer.color = color;
        }
    }
    /// <summary>
    /// Takes an index and sets the corresponding type. Sets the correct number of IOs.
    /// </summary>
    /// <param name="index">Index of the type we want to set</param>
    public void SetCorrectGateType(int index)
    {
        type = (Type)index;

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
    /// <summary>
    /// Sets the hitbox, inputs and outputs number, IOs positions and name of the component.
    /// </summary>
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
    /// <summary>
    /// Takes inputs and sets the outputs to their correct state according to the type of the component or to the truth table.
    /// </summary>
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
    /// <summary>
    /// Prepares a component to delete it. Basically removes all the linked wires.
    /// </summary>
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
    /// <summary>
    /// Each gate has his distinct color. This function returns it.
    /// </summary>
    /// <param name="type">Type we want the specific color from</param>
    /// <returns>Color corresponding to the entered parameter</returns>
    public Color GetColorFromType(Type type)
    {
        switch(type)
        {
            case Type.Buffer : return Color.black;
            case Type.NOT : return new Color(1f,0.27f,0f);
            case Type.AND : return Color.yellow;
            case Type.OR : return new Color(0.78f,0.082f,0.52f);
            case Type.XOR : return new Color(0f,0.9803f,0.603f);
            case Type.NAND : return Color.blue;
            case Type.NOR : return new Color(0.117f,0.564f,1f);
            case Type.XNOR : return new Color(1f,0.854f,0.725f);
        }

        return Color.white;
    }
}
