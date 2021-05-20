using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager singleton;
    public GameObject componentPrefab;
    public List<Component> components = new List<Component>();
    public List<Component> selectedComponents = new List<Component>();
    public List<Wire> wires = new List<Wire>();
    public List<IO> inputs = new List<IO>();
    public List<IO> outputs = new List<IO>();
    public bool leftShift;
    void Awake()
    {
        singleton = this;
        Application.targetFrameRate = 100;
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

            if(Input.GetKeyDown(KeyCode.H))
            {
                switch(Random.Range(0,3))
                {
                    case 0: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.ANSI; break;
                    case 1: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.IEC; break;
                    default: ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.DIN; break;
                }
                foreach(Component c in components)
                {
                    c.SetGateProperties();
                    c.UpdateCompGraphics();
                }
            }
        }


    }
    public void FullResresh()
    {
        
    }
    public void UpdateComponents()
    {
        foreach(Component c in components)
        {
            
        }
    }
    public void UpdateWires()
    {
        foreach(Wire w in wires)
        {
            w.UpdatePositions();
        }
    }
    public Component CreateComponent(Component.Type type)
    {
        Component newComp = Instantiate(componentPrefab, this.transform).GetComponent<Component>();
        newComp.type = type;
        newComp.name = newComp.type.ToString();
        components.Add(newComp);
        return newComp;
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
