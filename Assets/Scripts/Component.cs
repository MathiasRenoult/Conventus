using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    public enum Type {NOT,AND,OR,XOR,NAND,NOR,NXOR,Custom, Unset};
    Component[] inputs;
    Component[] outputs;
    string boolExpression;
    int[] truthTable;
    Type type = Type.Unset;

    private void Start()
    {
        
    }
}
