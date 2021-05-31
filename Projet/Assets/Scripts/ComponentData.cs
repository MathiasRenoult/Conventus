using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ComponentData
{
    public int type;
    public int inputs;
    public int outputs;
    public int[] truthTable;
    public ComponentData(Component c)
    {
        type = (int)c.type;
        inputs = (int)c.CountInputsOutputs().x;
        outputs = (int)c.CountInputsOutputs().y;
        truthTable = c.truthTable;
    }
}
