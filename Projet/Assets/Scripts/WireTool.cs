using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tool used to links IOs together.
/// </summary>
public class WireTool : MonoBehaviour
{
    public static WireTool singleton;
    Vector2 mousePos;
    public SpriteRenderer selectDot;
    public GameObject wirePrefab;
    public IO startIO;
    public IO currentIO;
    Vector2 currentPointPos;
    public Wire currentWire;
    public bool wiring;
    public Color onColor;
    public Color offColor;
    public float timeToProgress = 2f;
    public float sensitivity = 50f;
    void Awake()
    {
        singleton = this;
    }
    void Update()
    {
        mousePos = SelectionManager.singleton.mousePos;
        UpdateDotPosAndState();
        UpdateColors();
    }
    /// <summary>
    /// Update the "help dot" position, state and size, according to the cursor position and the camera zoom level.
    /// </summary>
    void UpdateDotPosAndState()
    {
        selectDot.gameObject.SetActive(false);
        selectDot.transform.localScale = new Vector2(Camera.main.orthographicSize / 30f, Camera.main.orthographicSize / 30f);
        currentIO = null;
        currentPointPos = Vector2.zero;
        IO closestPoint = new IO(false, false, Vector2.zero, null, null);
        float shortestDistance = Mathf.Infinity;
        bool foundIO = false;
        foreach(Component c in AppManager.singleton.components)
        {
            foreach(IO i in c.ios)
            {
                if((Input.mousePosition-Camera.main.WorldToScreenPoint(i.pos)).magnitude < sensitivity && (Input.mousePosition-Camera.main.WorldToScreenPoint(i.pos)).magnitude < shortestDistance)
                {
                    closestPoint = i;
                    shortestDistance = (Input.mousePosition-Camera.main.WorldToScreenPoint(i.pos)).magnitude;
                    foundIO = true;
                }
            }
        }
        foreach(CanvasIO c in AppManager.singleton.inputs)
        {
            if((Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude < sensitivity && (Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude < shortestDistance)
            {
                closestPoint = c.io;
                shortestDistance = (Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude;
                foundIO = true;
            }
        }
        foreach(CanvasIO c in AppManager.singleton.outputs)
        {
            if((Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude < sensitivity && (Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude < shortestDistance)
            {
                closestPoint = c.io;
                shortestDistance = (Input.mousePosition-Camera.main.WorldToScreenPoint(c.io.pos)).magnitude;
                foundIO = true;
            }
        }
        if(foundIO)
        {
            selectDot.gameObject.SetActive(true);
            selectDot.transform.position = closestPoint.pos;
            currentIO = closestPoint;
            currentPointPos = closestPoint.pos;
        }
       
        if(currentIO != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(AppManager.singleton.leftShift)
                {
                    DestroyWires(currentIO);
                }
                else
                {
                    StartWiring(); 
                }
            }
        }

        if(wiring)
        {
            UpdateWiring();

            if(Input.GetMouseButtonUp(0))
            {
                StopWiring();
            }
        }
    }
    /// <summary>
    /// Starts a new wire.
    /// </summary>
    public void StartWiring()
    {
        wiring = true;
        startIO = currentIO;
        currentWire = Instantiate(wirePrefab).GetComponent<Wire>();
        currentWire.start = currentIO;
    }
    /// <summary>
    /// Ends the wire. Either destroys it if the wire is invalid or creates it.
    /// </summary>
    public void StopWiring()
    {
        wiring = false;
        if(currentIO != null && currentIO != startIO && currentIO.input != startIO.input)
        {
            startIO.linkedIOS.Add(currentIO);
            currentIO.linkedIOS.Add(startIO);

           currentWire.end = currentIO;
           currentWire.start = startIO;
           currentWire.UpdatePositions();
           AppManager.singleton.wires.Add(currentWire);
        }
        else
        {
            Destroy(currentWire.gameObject);
        }
    }
    /// <summary>
    /// Update the position of the wire while we're still creating it.
    /// </summary>
    public void UpdateWiring()
    {
        if(startIO != null && currentWire != null && mousePos != null) currentWire.SetPositions(startIO.pos, mousePos);
    }
    /// <summary>
    /// Destroys all the wires linked to the specified IO.
    /// </summary>
    /// <param name="io">The specified IO</param>
    public void DestroyWires(IO io)
    {
        List<Wire> wires = new List<Wire>();
        foreach(Wire w in AppManager.singleton.wires)
        {
            if(w.start == io || w.end == io)
            {
                wires.Add(w);
            }
        }
        DestroyWires(wires.ToArray());
    }
    /// <summary>
    /// Takes an array of wires and destroys all of them.
    /// </summary>
    /// <param name="wires">The array of wires to destroy</param>
    public void DestroyWires(Wire[] wires)
    {
        foreach(Wire w in wires)
        {
            DestroyWire(w);
        }
    }
    /// <summary>
    /// Takes a specific wire and destroys it.
    /// </summary>
    /// <param name="wire">The wire to destroy</param>
    public void DestroyWire(Wire wire)
    {
        wire.end.linkedIOS.Remove(wire.start);
        wire.start.linkedIOS.Remove(wire.end);
        AppManager.singleton.wires.Remove(wire);
        Destroy(wire.gameObject);
    }
    /// <summary>
    /// Updates the colors of the wires according to the simulation computed states. (green or red, on or off)
    /// </summary>
    public void UpdateColors()
    {
        if(Simulation.singleton.simulating)
        {
            foreach(Wire w in AppManager.singleton.wires)
            {
                if(!w.start.input)
                {
                    w.SetState(w.start.state);
                }
                else
                {
                    w.SetState(w.end.state);
                }  
            }
        }
        else
        {
            if(currentIO == null)
            {
                foreach(Wire w in AppManager.singleton.wires)
                {
                    w.SetColors(Color.white, Color.white);
                }
            }
            else
            {
                foreach(Wire w in AppManager.singleton.wires)
                {
                    if(w.end == currentIO || w.start == currentIO)
                    {
                        w.SetColors(Color.red, Color.red);
                        w.wire.sortingOrder = 10;
                    }
                    else
                    {
                        w.SetColors(new Color(1f,1f,1f,0.3f), new Color(1f,1f,1f,0.3f));
                        w.wire.sortingOrder = 0;
                    }
                }
            }
        }
    }
}
