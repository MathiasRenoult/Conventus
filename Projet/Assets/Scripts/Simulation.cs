using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public static Simulation singleton;
    public bool simulating;

    private void Awake()
    {
        singleton = this;
    }
    void Update()
    {
        if(simulating)
        {
            RefreshState();
        }
    }

    public void RefreshState()
    {
        List<IO> nextStep = new List<IO>();
        foreach(CanvasIO c in AppManager.singleton.inputs)
        {
            if(c.io.linkedIOS.Count > 0)
            {
                foreach(IO i in c.io.linkedIOS)
                {
                    i.state = c.io.state;
                    nextStep.Add(i);
                }
            }
        }
        ComputeNextStep(nextStep);
    }

    public void ComputeNextStep(List<IO> ios)
    {
        List<IO> nextStep = new List<IO>();
        foreach(IO i in ios)
        {
            if(i.component != null)
            {
                i.component.ComputeOutputStates();
                foreach(IO j in i.component.ios)
                {
                    if(!j.input && j.linkedIOS.Count > 0)
                    {
                        foreach(IO k in j.linkedIOS)
                        {
                            k.state = j.state;
                            nextStep.Add(k);
                        }
                    }
                }
            }
        }
        if(nextStep.Count > 0)
        {
            ComputeNextStep(nextStep);
        }
    }
}
