using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IO
{
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
