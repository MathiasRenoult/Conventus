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

        if(currentIO != null)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartWiring();
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
        if(currentIO != null && currentIO != startIO)
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
        currentWire.SetPositions(startIO.pos, mousePos);
    }
}
