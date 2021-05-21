using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float sensitivity = 0.5f;
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

    void UpdateDotPosAndState()
    {
        selectDot.gameObject.SetActive(false);
        currentIO = null;
        currentPointPos = Vector2.zero;
        foreach(Component c in AppManager.singleton.components)
        {
            foreach(IO i in c.ios)
            {
                if((mousePos-i.pos).magnitude < sensitivity)
                {
                    selectDot.gameObject.SetActive(true);
                    selectDot.transform.position = i.pos;
                    currentIO = i;
                    currentPointPos = i.pos;
                }
            }
        }
        foreach(CanvasIO c in AppManager.singleton.inputs)
        {
            if((c.io.pos - mousePos).magnitude < sensitivity)
            {
                selectDot.gameObject.SetActive(true);
                selectDot.transform.position = c.io.pos;
                currentIO = c.io;
                currentPointPos = c.io.pos;
            }
        }
        foreach(CanvasIO c in AppManager.singleton.outputs)
        {
            if((c.io.pos - mousePos).magnitude < sensitivity)
            {
                selectDot.gameObject.SetActive(true);
                selectDot.transform.position = c.io.pos;
                currentIO = c.io;
                currentPointPos = c.io.pos;
            }
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

    public void StartWiring()
    {
        wiring = true;
        startIO = currentIO;
        currentWire = Instantiate(wirePrefab).GetComponent<Wire>();
        currentWire.start = currentIO;
    }

    public void StopWiring()
    {
        wiring = false;
        if(currentIO != null && currentIO != startIO && currentIO.input != startIO.input)
        {
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

    public void UpdateWiring()
    {
        if(startIO != null && currentWire != null && mousePos != null) currentWire.SetPositions(startIO.pos, mousePos);
    }
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
    public void DestroyWires(Wire[] wires)
    {
        foreach(Wire w in wires)
        {
            DestroyWire(w);
        }
    }
    public void DestroyWire(Wire wire)
    {
        AppManager.singleton.wires.Remove(wire);
        Destroy(wire.gameObject);
    }

    public void UpdateColors()
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
