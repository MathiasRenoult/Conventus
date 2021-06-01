using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    public static Simulation singleton;
    public List<IO> alreadyComputed = new List<IO>();
    public bool simulating;
    public int stepLimit = 1000;
    public int stepCounter;

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
        stepCounter = 0;
        alreadyComputed.Clear();
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
        stepCounter++;
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
        List<IO> toRemove = new List<IO>();
        foreach(IO i in nextStep)
        {
            if(alreadyComputed.Find(x => x.pos == i.pos) != null)
            {
                toRemove.Add(i);
            }
        }
        foreach(IO i in toRemove)
        {
            nextStep.Remove(i);
        }
        alreadyComputed.AddRange(nextStep);
        if(nextStep.Count > 0 && stepCounter < stepLimit)
        {
            ComputeNextStep(nextStep);
        }
    }
}
