using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasIO : MonoBehaviour
{
    [SerializeField]
    public IO io;
    public UnityEngine.UI.Image image;

    public void ChangeState()
    {
        if(!io.input)
        {
            io.state = !io.state;            
        }

        UpdateColor();
    }  

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
