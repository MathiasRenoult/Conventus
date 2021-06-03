using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// This class is used by the nesting panel only. It's used when you create a new component.
/// </summary>
public class NestingPanel : MonoBehaviour
{   
    public TMP_InputField nameInputField;
    public TMP_InputField colorInputField;
    public Toggle randomColorToggle;
    /// <summary>
    /// Flips the state (visibility) of the panel; on / off.
    /// </summary>
    public void EnableDisablePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    /// <summary>
    /// Called when the "create" button is clicked. It takes the elements on the canvas, and the panel parameters
    /// and creates a new component stored in the files.
    /// </summary>
    public void CreateComponent()
    {
        bool correct = true;
        if(nameInputField.text == "")
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            nameInputField.colors = colorBlock;
            correct = false;
        }

        if(StringToColor(colorInputField.text) == new Color(-1f, -1f, -1f) && !randomColorToggle.isOn)
        {
            ColorBlock colorBlock = new ColorBlock();
            colorBlock.normalColor = Color.red;
            colorInputField.colors = colorBlock;
            correct  = false;
        }
        if(correct)
        {
            print("This is correct !");
            Component newComp = AppManager.singleton.CreateComponent(Component.Type.Custom, StringToColor(colorInputField.text), nameInputField.text, AppManager.singleton.inputs.Count, AppManager.singleton.outputs.Count, AppManager.singleton.ComputeCurrentTruthTable());
            newComp.gameObject.SetActive(false);
            ComponentButtonManager.singleton.registeredComponents.Add(newComp);
            print("New component added !");
        }

        SaveManager.singleton.SaveComponents();
    }
    /// <summary>
    /// Takes a string to the standard html hexadecimal format (#0123def) and converts it the correct color.
    /// </summary>
    /// <param name="colorString">The string we want to parse</param>
    /// <returns>The color interpreted from the string</returns>
    public Color StringToColor(string colorString)
    {
        Color colorColor;
        bool correct = ColorUtility.TryParseHtmlString(colorString, out colorColor);;
        if(correct)
        {
            return colorColor;
        }
        else
        {
            return new Color(-1f, -1f, -1f);
        }
    }
}
