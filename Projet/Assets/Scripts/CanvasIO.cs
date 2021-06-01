using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is used on inputs and outputs of the canvas.
/// </summary>
public class CanvasIO : MonoBehaviour
{
    [SerializeField]
    public IO io;
    public UnityEngine.UI.Image image;
    /// <summary>
    /// Flips the state of the associated IO.
    /// </summary>
    public void ChangeState()
    {
        if(!io.input)
        {
            io.state = !io.state;            
        }

        UpdateColor();
    }  
    /// <summary>
    /// Updates the color of the element according to its state.
    /// </summary>
    public void UpdateColor()
    {
        if((!io.input || io.linkedIOS.Count > 0) && Simulation.singleton.simulating)
        {
            if(io.state)
            {
                image.color = new Color(35f/255f, 226f/255f, 0f);
            }
            else
            {
                image.color = new Color(229f/255f, 65f/255f, 0f);
            }
        }
        else
        {
            image.color = new Color(0.3f,0.3f,0.3f);
        }
    } 
}
