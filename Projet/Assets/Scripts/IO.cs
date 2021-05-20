using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IO
{
    public IO(bool input, bool state, Vector2 pos, IO io)
    {
        this.input = input;
        this.state = state;
        this.pos = pos;
        this.linkedIO = io;
    }

    /**
     is input ?
    */
    public bool input; 
    public bool state;
    public Vector2 pos;
    public IO linkedIO;
}
