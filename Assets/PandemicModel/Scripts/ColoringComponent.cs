using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColoringComponent : AComponent
{

    public Color color;
    public Renderer coloringTarget;
    
    public Color AgentColor
    {
        get
        {
            return color;
        }
        set
        {
            color = value;

            if (coloringTarget != null)
            {
                coloringTarget.material.color = color; 
            } else
            {
                Renderer[] rs = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in rs)
                {
                    r.material.color = color;
                }
            }
        }
    }

    void Awake(){
        AgentColor = color;
    }

    public void SetColor(Color c)
    {
        AgentColor = c;
    }
    
    
}
