using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ComponentButtonManager : MonoBehaviour
{
    public static ComponentButtonManager singleton;
    public Button createAndSaveButton;
    public Button componentMenuButton;
    public GameObject componentMenuPanel;
    public int itemLimit = 12;
    public float spacing = 120f;
    public GameObject componentPrefab;
    public List<ComponentButton> buttons = new List<ComponentButton>();
    public List<Component> registeredComponents;
    public List<Component> favoriteComponents;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
    }
    public void RefreshButtons()
    {
        buttons.Clear();

        for(int i = 0; i<favoriteComponents.Count && i<itemLimit;i++)
        {
            ComponentButton newButt = Instantiate(componentPrefab, this.transform).GetComponent<ComponentButton>();
            newButt.transform.position = new Vector3(this.transform.position.x - (this.GetComponent<RectTransform>().sizeDelta.x/2) + (i * spacing), this.transform.transform.position.y, 0f);            
            newButt.linkedComponent = favoriteComponents[i];
            newButt.SetText(favoriteComponents[i].compName);
            newButt.SetColor(Color.red);
            buttons.Add(newButt);
        }
        componentMenuButton.transform.position = new Vector3(this.transform.position.x - (this.GetComponent<RectTransform>().sizeDelta.x/2) + (favoriteComponents.Count * spacing), this.transform.transform.position.y, 0f);
    }

    public void OpenCloseComponentMenu()
    {
        componentMenuPanel.SetActive(!componentMenuPanel.activeSelf);
        if(componentMenuPanel.activeSelf)
        {
            ComponentMenuManager.singleton.InstantiateRegisteredComponents();            
        }
    }
}
