using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// The App Manager is the central class of the application. It manages components on the canvas, 
/// selected components, wires, some UI, canvas IO, mouse position and some keystrokes.
/// </summary>
public class AppManager : MonoBehaviour
{
    public static AppManager singleton;
    public GameObject componentPrefab;
    public List<Component> components = new List<Component>();
    public List<Component> selectedComponents = new List<Component>();
    public List<Wire> wires = new List<Wire>();
    [SerializeField]
    public GameObject inputOutputPrefab;
    public GameObject leftContainer;
    public Transform leftBorderTransform;
    public List<CanvasIO> inputs = new List<CanvasIO>();
    public bool leftBorder; // true if pointer is on the left border
    [SerializeField]
    public GameObject rightContainer;
    public Transform rightBorderTransform;
    public List<CanvasIO> outputs = new List<CanvasIO>();
    public Vector2 mousePos;
    public Vector2 oldMousePos;
    public bool rightBorder; // true if pointer is on right border
    public bool leftShift;
    void Awake()
    {
        singleton = this;
        Application.targetFrameRate = 100;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
           oldMousePos = mousePos; 
        }
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(!leftShift)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * (Camera.main.orthographicSize/10);
            UpdateCanvasIO();
        }

        if(Input.GetMouseButton(1))
        {
            Camera.main.transform.position += (Vector3)(oldMousePos - mousePos);
            UpdateCanvasIO();
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            leftShift = false;
        }

        if(Input.anyKey)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                leftShift = true;
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                switch(UnityEngine.Random.Range(0,3))
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
    /// <summary>
    /// This function places the inputs and outputs switches on the left and right border of the screen.
    /// It dynamically generates them by taking the current number of wanted IO. The function "UpdateWires()"
    /// is called at the end.
    /// </summary>
    public void UpdateCanvasIO()
    {
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.UpdateColor();
            index++;
        }
        index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.UpdateColor();
            index++;
        }
        
        UpdateWires();
    }
    /// <summary>
    /// Flips the "leftBorder" boolean attribut of the class.
    /// </summary>
    public void SetOnOffLeftBorder()
    {
        leftBorder = !leftBorder;
    }
    /// <summary>
    /// Flips the "rightBorder" boolean attribut of the class.
    /// </summary>
    public void SetOnOffRightBorder()
    {
        rightBorder = !rightBorder;
    }
    /// <summary>
    /// Adds an input switch to the left border. It is not placed at the correct position yet, "UpdateCanvasIO()"
    /// will correct it later.
    /// </summary>
    public void AddInput()
    {
        CanvasIO newInput = Instantiate(inputOutputPrefab, leftBorderTransform).GetComponent<CanvasIO>();
        inputs.Add(newInput);
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.io.input = false;
            index++;
        }
    }
    /// <summary>
    /// Deletes and input from the left border. The inputs left are then replaced at the correct position
    /// next time "UpdateCanvasIO()" is called.
    /// </summary>
    public void DeleteInput()
    {
        CanvasIO toDelete = inputs[inputs.Count-1];
        toDelete.io.FlushIO();
        inputs.Remove(toDelete);
        Destroy(toDelete.gameObject);
        int index = 0;
        foreach(CanvasIO c in inputs)
        {
            c.transform.position = new Vector2(leftBorderTransform.transform.position.x, leftBorderTransform.position.y - (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (leftBorderTransform.GetComponent<RectTransform>().rect.size.y/(inputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x + (leftBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            index++;
        }
    }
    /// <summary>
    /// Adds an output switch to the right border. It is not placed at the correct position yet, "UpdateCanvasIO()"
    /// will correct it later.
    /// </summary>
    public void AddOutput()
    {
        CanvasIO newOutput = Instantiate(inputOutputPrefab, rightBorderTransform).GetComponent<CanvasIO>();
        outputs.Add(newOutput);
        int index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            c.io.input = true;
            c.ChangeState();
            index++;
        }
    }
    /// <summary>
    /// Deletes and output from the right border. The inputs left are then replaced at the correct position
    /// next time "UpdateCanvasIO()" is called.
    /// </summary>
    public void DeleteOutput()
    {
        CanvasIO toDelete = outputs[outputs.Count-1];
        toDelete.io.FlushIO();
        outputs.Remove(toDelete);
        Destroy(toDelete.gameObject);
        int index = 0;
        foreach(CanvasIO c in outputs)
        {
            c.transform.position = new Vector2(rightBorderTransform.transform.position.x, rightBorderTransform.position.y - (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/2) + (rightBorderTransform.GetComponent<RectTransform>().rect.size.y/(outputs.Count + 1))*(index+1));
            c.io.pos = new Vector2(c.transform.position.x - (rightBorderTransform.GetComponent<RectTransform>().rect.size.x/2), c.transform.position.y);
            c.io.pos = Camera.main.ScreenToWorldPoint(c.io.pos);
            index++;
        }
    }
    /// <summary>
    /// Calls the "UpdatePositions()" and "UpdateSize()" function for each wire int the "wires" list of the class.
    /// </summary>
    public void UpdateWires()
    {
        foreach(Wire w in wires)
        {
            w.UpdatePositions();
            w.UpdateSize();
        }
    }
    /// <summary>
    /// Creates a new component to be used.
    /// </summary>
    /// <param name="type"> Either a logic-gate-type or Custom</param>
    /// <param name="color"> Color used by the custom-type asset</param>
    /// <param name="name"> Name of the component</param>
    /// <param name="inputs">Number of inputs</param>
    /// <param name="outputs">Number of outputs</param>
    /// <param name="truthTable">Truth table of the component as a simple int array. Would be converted to a bool array later.</param>
    /// <returns></returns>
    public Component CreateComponent(Component.Type type, Color color, string name, int inputs = 0, int outputs = 0, int[] truthTable = null)
    {
        Component newComp = Instantiate(componentPrefab, this.transform).GetComponent<Component>();
        newComp.type = type;
        if(name == null)
        {
            newComp.name = newComp.compName = newComp.type.ToString();
            print("Name is null !");
        }
        else
        {
            newComp.name = newComp.compName = name;
            print(newComp.name);
        }
        if(color != new Color(-1f, -1f, -1f))
        {
            newComp.color = color;
        }
        else
        {
            newComp.color = Color.HSVToRGB((Mathf.Abs(newComp.name.GetHashCode())%512)/512f, 0.9f, 0.7f);
        }
        newComp.inputs = inputs;
        newComp.outputs = outputs;
        if(truthTable != null)
        {
            newComp.truthTable = truthTable;
        }
        else
        {
            print("Truth table is null !");
        }
        newComp.SetCorrectGateType((int)newComp.type);
        return newComp;
    }
    /// <summary>
    /// Overload of the function taking a simple component as parameter.
    /// </summary>
    /// <param name="comp">Component used as model</param>
    /// <returns></returns>
    public Component CreateComponent(Component comp)
    {
        return CreateComponent(comp.type, comp.color, comp.name, comp.inputs, comp.outputs, comp.truthTable);
    }
    /// <summary>
    /// Overload of the function taking a ComponentData parameter and a string.
    /// </summary>
    /// <param name="data">Data used as model to create a new component</param>
    /// <param name="name">Name of the new component</param>
    /// <returns></returns>
    public Component CreateComponent(ComponentData data, string name)
    {
        return CreateComponent((Component.Type)data.type, new Color(-1f, -1f, -1f), name, data.inputs, data.outputs, data.truthTable);
    }
    /// <summary>
    /// This function uses the already set truth table to compute the correct outputs. 
    /// </summary>
    /// <returns></returns>
    public int[] ComputeCurrentTruthTable()
    {
        int size = (int)Mathf.Pow(2, inputs.Count);
        int[] truthTable = new int[size * outputs.Count];
        for(int i = 0; i < size; i++)
        {
            string binary = Convert.ToString(i, 2).PadLeft(inputs.Count, '0');
            for(int j = 0; j < binary.Length; j++)
            {
                inputs[j].io.state = binary[j] == '1' ? true : false;
            }
            Simulation.singleton.RefreshState();
            for(int j = 0; j<outputs.Count; j++)
            {
                truthTable[(i*outputs.Count)+j] = outputs[j].io.state == true ? 1 : 0;
            } 
        }
        
        return truthTable;
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
