using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HumanBehavior : ABehavior
{

    private bool infected = false;
    private int timeInfected = 0;
    public int latentPeriod = 8;
    public float radius = 1;
    //
    public ZombieBehavior zombiePrefab;
    //
    private Color defaultColor;
    private ColoringComponent cb;
    private SpeedDirectionBehavior sp;
    
    public void Infect()
    {
        infected = true;
    }
    
    public void DisInfect()
    {
        infected = false;
        timeInfected = 0;
    }

    public override void Initialize()
    {
        cb = GetComponent<ColoringComponent>();
        defaultColor = cb.AgentColor;
        sp = GetComponent<SpeedDirectionBehavior>();
    }
    
    public override void Begin()
    {
    }

    public override void Step()
    {
        if (infected)
        {
            timeInfected++;
        }
        
        if (infected)
        {
            cb.AgentColor = Color.yellow;
        } else
        {
            cb.AgentColor = defaultColor;
        } 

        List<ZombieBehavior> zombies = GetAgentsAroundPosition<ZombieBehavior> (transform.position, radius, false, true).
            OrderBy(x=>Vector3.Distance(x.transform.position, transform.position)).ToList();
        if(zombies.Count > 0){
            sp.LookAt(zombies[0].transform.position, true);
            sp.Turn(0, 180, 0); 
        }

    }
    
    public override void Commit()
    {
        if (timeInfected > latentPeriod)
        {
            AAgent a = CreateAgent(zombiePrefab.AttachedAgent);
            a.transform.position = transform.position;
            a.gameObject.name = a.gameObject.name + " pre Human "+a.transform.position;

            ResignAgent(AttachedAgent);
        }
    }
    
    public override void End()
    {
    }
    
    public override void Dispose()
    {
    }

}
