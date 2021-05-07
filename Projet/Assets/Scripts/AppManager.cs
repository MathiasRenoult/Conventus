using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager singleton;
    public List<Component> components = new List<Component>();
    public List<Component> selectedComponents = new List<Component>();
    public List<IO> inputs = new List<IO>();
    public List<IO> outputs = new List<IO>();
    public bool leftShift;
    public bool ansi = true; //if true, uses ANSI instead of IEC gates standard
    void Awake()
    {
        singleton = this;
    }

    void Update()
    {
        if(Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                leftShift = true;
            }

            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                leftShift = false;
            }
        }
    }

    public void SelectComponent(Component c)
    {
        c.held = !c.held;
        if(c.held)
        {
            selectedComponents.Add(c);
        }
        else
        {
            selectedComponents.Remove(c);
        }
    }

    public void DeselectAllSelectedComponents()
    {
        foreach(Component c in selectedComponents)
        {
            c.held = false;
        }
        selectedComponents.Clear();
    }
}
