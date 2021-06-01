using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpBarButton : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject tutorialPanel;
    public void OpenCloseContainer(Transform container)
    {
        container.gameObject.SetActive(!container.gameObject.activeSelf);
    }
    //Fichiers
    public void SaveButton()
    {
        SaveManager.singleton.SaveComponents();
    }
    public void LoadButton()
    {
        SaveManager.singleton.LoadComponents();
    }
    public void QuitButton()
    {
        SaveManager.singleton.SaveComponents();
        Application.Quit();
    }
    //Edit
    public void SimulateButton()
    {
        Simulation.singleton.simulating = !Simulation.singleton.simulating;
    }
    public void ClearCanvasButton()
    {
        AppManager.singleton.ClearCanvas();
    }
    public void PNGExport()
    {
        AppManager.singleton.Capture();
    }
    //View
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
    public void ANSINorm()
    {
        SetNorm(0);
    }
    public void IECNorm()
    {
        SetNorm(1);
    }
    public void DINNorm()
    {
        SetNorm(2);
    }
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
    public void Tutorial()
    {
        tutorialPanel.SetActive(!tutorialPanel.activeSelf);
    }
    public void Credits()
    {
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }
}
