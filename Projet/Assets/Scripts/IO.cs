using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IO
{
    public IO(bool input, bool state, Vector2 pos, Component component, IO io)
    {
        this.input = input;
        this.state = state;
        this.pos = pos;
        this.component = component;
        this.linkedIO = io;
    }
    
    /**
     is input ?
    */
    public bool input; 
    public bool state;
    public Vector2 pos;
    public Component component;
    public IO linkedIO;
}
