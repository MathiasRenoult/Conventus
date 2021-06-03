using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is set on each wire prefab. Wires link IOs together.
/// </summary>
public class Wire : MonoBehaviour
{
    public LineRenderer wire;
    public IO start;
    public IO end;
    public bool progressing;
    public bool state;
    /// <summary>
    /// Sets the path of the wire from the specified start and end positions.
    /// </summary>
    /// <param name="start">The start position of the wire</param>
    /// <param name="end">The end position of the wire</param>
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
    /// <summary>
    /// Deprecated. Set the state of the wire and the amount of "current" progression of it.
    /// </summary>
    /// <param name="state">States we'd like to set the wire to</param>
    /// <param name="progression">The current progression of the state in the wire</param>
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
    /// <summary>
    /// Sets the color of the wire, gradients between start and finish color
    /// </summary>
    /// <param name="start">Start color of the wire</param>
    /// <param name="end">End color of the wire</param>
    public void SetColors(Color start, Color end)
    {
        this.wire.startColor = start;
        this.wire.endColor = end;
    }
    /// <summary>
    /// Updates the positions of the wire according to the registered positions in the class. (Graphical update)
    /// </summary>
    public void UpdatePositions()
    {
        SetPositions(start.pos, end.pos);
    }
    /// <summary>
    /// Updates the width of the wire based on the camer zoom level.
    /// </summary>
    public void UpdateSize()
    {
        wire.startWidth = Camera.main.orthographicSize / 200f;
        wire.endWidth = Camera.main.orthographicSize / 200f;
    }
}
