using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// This class cannot be instantiated. It is used as a temporary class to convert components to JSON and JSON to components.
/// </summary>
[Serializable]
public class ComponentData
{
    public int type;
    public int inputs;
    public int outputs;
    public int[] truthTable;
    /// <summary>
    /// Constructor converting a component to a componentData ready to be serialized.
    /// </summary>
    /// <param name="c">Component used as model</param>
    public ComponentData(Component c)
    {
        type = (int)c.type;
        inputs = (int)c.CountInputsOutputs().x;
        outputs = (int)c.CountInputsOutputs().y;
        truthTable = c.truthTable;
    }
}
