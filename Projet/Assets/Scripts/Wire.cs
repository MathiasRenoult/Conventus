using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public LineRenderer wire;
    public IO start;
    public IO end;
    public bool progressing;
    public bool state;

    public void SetPositions(Vector2 start, Vector2 end)
    {
        Vector3[] pos = new Vector3[4];
        pos[0] = start;
        pos[3] = end;
        if(start.x < end.x) {
            pos[1] = new Vector3((start.x + end.x) / 2, start.y);
            pos[2] = new Vector3((start.x + end.x) / 2, end.y);
        } else {
            pos[1] = new Vector3(start.x, (start.y + end.y) / 2);
            pos[2] = new Vector3(end.x, (start.y + end.y) / 2);
        }
        wire.positionCount = 4;
        wire.SetPositions(pos);
    }
    public void SetState(bool state, float progression = 100f)
    {
        this.state = state;
        if(Simulation.singleton.simulating)
        {
            wire.startColor = state == true ? WireTool.singleton.onColor : WireTool.singleton.offColor;
            wire.endColor = state == true ? WireTool.singleton.onColor : WireTool.singleton.offColor;
        }
        else
        {
            wire.startColor = Color.white;
            wire.endColor = Color.white;
        }
    }

    public void SetColors(Color start, Color end)
    {
        this.wire.startColor = start;
        this.wire.endColor = end;
    }

    public void UpdatePositions()
    {
        SetPositions(start.pos, end.pos);
    }
    public void UpdateSize()
    {
        wire.startWidth = Camera.main.orthographicSize / 200f;
        wire.endWidth = Camera.main.orthographicSize / 200f;
    }
}
