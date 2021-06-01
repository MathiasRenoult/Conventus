using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used on gates buttons. It creates the correct gate when pressed.
/// </summary>
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
    /// <summary>
    /// Sets the text of the button.
    /// </summary>
    /// <param name="text">Text to apply</param>
    public void SetText(string text)
    {
        this.text.text = text;
    }
    /// <summary>
    /// Sets the color of the button.
    /// </summary>
    /// <param name="color">Color to apply</param>
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
    /// <summary>
    /// Instantiates the correct gate according to the "linkedComponent" attribute.
    /// </summary>
    public void InstantiateComponent()
    {
        Component instComp = Instantiate(linkedComponent);
        AppManager.singleton.components.Add(instComp);
        AppManager.singleton.SelectComponent(instComp);
        instComp.gameObject.SetActive(true);
        instComp.shapeRenderer.color = instComp.color;
    }
}
