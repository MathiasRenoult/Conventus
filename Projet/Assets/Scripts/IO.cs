using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is not a child of MonoBehavior.cs. It means that it cannot be instantiated. It's used on every input or
/// output we'd like on the canevas.
/// </summary>
[System.Serializable]
public class IO
{
    /// <summary>
    /// Constructs a new IO.
    /// </summary>
    /// <param name="input">If true, the IO class will act as an input</param>
    /// <param name="state">Determines the state of the IO</param>
    /// <param name="pos">Position of the IO on the canvas (world coordinates)</param>
    /// <param name="ios">List of all IOs this instance is linked to</param>
    /// <param name="component">Component that contains this IO</param>
    public IO(bool input, bool state, Vector2 pos, List<IO> ios, Component component)
    {
        this.input = input;
        this.state = state;
        this.pos = pos;
        this.linkedIOS = ios;
        this.component = component;
    }

    /**
     is input ?
    */
    public bool input; 
    public bool state;
    public Vector2 pos;
    [SerializeField]
    public List<IO> linkedIOS;
    public Component component;
    /// <summary>
    /// Prepares the IO to be destroyed; it removes all connections and wires.
    /// </summary>
    public void FlushIO()
    {
        List<Wire> toDestroy = new List<Wire>();
        foreach(Wire w in AppManager.singleton.wires)
        {
            foreach(IO i in w.start.linkedIOS)
            {
                if(i == this)
                {
                    toDestroy.Add(w);
                }
            }
            foreach(IO i in w.end.linkedIOS)
            {
                if(i == this)
                {
                    toDestroy.Add(w);
                }
            }
            
        }
        WireTool.singleton.DestroyWires(toDestroy.ToArray());
    }
}
