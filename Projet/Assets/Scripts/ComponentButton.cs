using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentButton : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public UnityEngine.UI.Image spriteRenderer;
    public Component linkedComponent;

    void Awake()
    {
        if(text == null)
        {
            text = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        }
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<UnityEngine.UI.Image>();
        }
    }

    void Start()
    {
        SetText(linkedComponent.type.ToString());
    }
    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public void InstantiateComponent()
    {
        Component instComp = Instantiate(linkedComponent);
        AppManager.singleton.components.Add(instComp);
        AppManager.singleton.SelectComponent(instComp);
        instComp.gameObject.SetActive(true);
    }
}
