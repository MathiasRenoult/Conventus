using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Component : MonoBehaviour
{
    public enum Type {Buffer, NOT, AND, OR, XOR, NAND, NOR, XNOR, Custom, Unset};
    public List<IO> inputs = new List<IO>();
    public List<IO> outputs = new List<IO>();
    public SpriteRenderer renderer;
    public string boolExpression;
    public int[] truthTable;
    public bool truthTableDone = false;
    public Type type = Type.Unset;
    public bool held;
    public Vector2 heldPoint;
    BoxCollider2D compCollider;

    private void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
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
    }

    float RoundToNearest(float n, float x) 
    {
        return Mathf.Round(n / x) * x;
    }

    public void ComputeTruthTable()
    {

    }
    public void UpdateState()
    {
        if(type == Type.Custom)
        {
            if(truthTableDone)
            {
                
            }
            else
            {
                ComputeTruthTable();
            }
        }
        else
        {
            switch(type)
            {
                case Type.Buffer : outputs[0].state = inputs[0].state; break;
                case Type.NOT : outputs[0].state = !inputs[0].state; break;
                case Type.AND : outputs[0].state = inputs[0].state && inputs[1].state; break;
                case Type.NAND : outputs[0].state = !(inputs[0].state && inputs[1].state); break;
                case Type.OR : outputs[0].state = inputs[0].state || inputs[1].state; break;
                case Type.NOR : outputs[0].state = !(inputs[0].state || inputs[1].state); break;
                case Type.XOR : outputs[0].state = inputs[0].state != inputs[1].state; break;
                case Type.XNOR : outputs[0].state = (inputs[0].state == inputs[1].state); break;
            }
        }
    }

    public void UpdateCompGraphics()
    {
        SetCompGraphics((int)type);
    } 
    public void SetCompGraphics(int index)
    {
        print(index);
        if(AppManager.singleton.ansi)
        {
            renderer.sprite = ComponentsGraphicManager.singleton.spritesANSI[index];
        }
        else
        {
            renderer.sprite = ComponentsGraphicManager.singleton.spritesIEC[index];
        }
    }
}
