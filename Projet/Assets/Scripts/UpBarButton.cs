using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the buttons at the top of the screen
/// </summary>
public class UpBarButton : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject tutorialPanel;
    /// <summary>
    /// Flips the state (visibility) of the GameObject attached to the specified Transform.
    /// </summary>
    /// <param name="container">Reference to the transform we'd like to flip the state</param>
    public void OpenCloseContainer(Transform container)
    {
        container.gameObject.SetActive(!container.gameObject.activeSelf);
    }
    //Fichiers
    /// <summary>
    /// Saves all components to the HDD.
    /// </summary>
    public void SaveButton()
    {
        SaveManager.singleton.SaveComponents();
    }
    /// <summary>
    /// Loads every components from the HDD.
    /// </summary>
    public void LoadButton()
    {
        SaveManager.singleton.LoadComponents();
    }
    /// <summary>
    /// Saves then closes the app.
    /// </summary>
    public void QuitButton()
    {
        SaveManager.singleton.SaveComponents();
        Application.Quit();
    }
    //Edit
    /// <summary>
    /// Starts and stop the simulation.
    /// </summary>
    public void SimulateButton()
    {
        Simulation.singleton.simulating = !Simulation.singleton.simulating;
    }
    /// <summary>
    /// Clears every component and wires from the canvas.
    /// </summary>
    public void ClearCanvasButton()
    {
        AppManager.singleton.ClearCanvas();
    }
    /// <summary>
    /// Exports the current view to the HDD as PNG.
    /// </summary>
    public void PNGExport()
    {
        AppManager.singleton.Capture();
    }
    //View
    /// <summary>
    /// Centers the view on the components.
    /// </summary>
    public void Center()
    {
        float xValue = 0;
        float yValue = 0;
        foreach(Component c in AppManager.singleton.components)
        {
            xValue += c.transform.position.x;
            yValue += c.transform.position.y;
        }
        Vector3 pos = new Vector3(xValue / AppManager.singleton.components.Count, yValue / AppManager.singleton.components.Count, -1f);
        Camera.main.transform.position = pos;
    }
    /// <summary>
    /// Sets all gates to the ANSI norm.
    /// </summary>
    public void ANSINorm()
    {
        SetNorm(0);
    }
    /// <summary>
    /// Sets all gates to the IEC norm.
    /// </summary>
    public void IECNorm()
    {
        SetNorm(1);
    }
    /// <summary>
    /// Sets all gates to the DIN norm.
    /// </summary>
    public void DINNorm()
    {
        SetNorm(2);
    }
    /// <summary>
    /// Sets all gates to the specified norm.
    /// </summary>
    /// <param name="i">Wanted norm as int. (0 = ANSI, 1 = IEC, 2 = DIN)</param>
    public void SetNorm(int i)
    {
        switch(i)
        {
            case 0 : ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.ANSI; break;
            case 1 : ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.IEC; break;
            case 2 : ComponentsGraphicManager.singleton.standard = ComponentsGraphicManager.LogicGatesStandard.DIN; break;
            default: print("Something went wrong !"); break;
        }
        
        foreach(Component c in AppManager.singleton.components)
        {
            c.SetGateProperties();
            c.UpdateCompGraphics();
            c.Move(c.transform.position, true);
        }
    }

    //About
    /// <summary>
    /// Flips the state (visibility) of the tutorial panel.
    /// </summary>
    public void Tutorial()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
    /// <summary>
    /// Flips the state (visibility) of the credits panel.
    /// </summary>
    public void Credits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
}
