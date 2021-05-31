using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComponentMenuItem : MonoBehaviour
{
    public Button editButton;
    public Button deleteButton;
    public TMPro.TextMeshProUGUI inputText;
    public TMPro.TextMeshProUGUI outputText;
    public TMPro.TextMeshProUGUI nameText;
    public Color color;
    public Image background;
    public Component linkedComponent;

    public void InstantiateComponent()
    {
        Component instComp = Instantiate(linkedComponent);
        AppManager.singleton.components.Add(instComp);
        //AppManager.singleton.SelectComponent(instComp);
        instComp.gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        ComponentMenuManager.singleton.componentMenuPanel.gameObject.SetActive(false);
    } 
}
