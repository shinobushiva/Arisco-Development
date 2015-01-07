using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;
using System.Linq;
using System;

///<summery>
///Component class
///
///@author shiva
///
///</summery>
public abstract class AComponent : AriscoTools
{

    void Start(){
    }

    ///<summery>
    ///Attached Agent
    ///</summery>
    private AAgent attachedAgent;

    public AAgent AttachedAgent
    {
        get
        {
            if (attachedAgent == null)
                attachedAgent = GetComponent<AAgent>();
            
            return attachedAgent;
        }
        set{ attachedAgent = value; }
    }

    public World AttachedWorld
    {
        get
        {
            return AttachedAgent.World;
        }
    }

    public AAgent CreateAgent(AAgent agent)
    {
        return CreateAgent(AttachedWorld, agent, Vector3.zero);
    }

    public AAgent CreateAgent(AAgent agent, Vector3 pos)
    {
        return CreateAgent(AttachedWorld, agent, pos);
    }

    public void ResignAgent(AAgent agent)
    {
        ResignAgent(AttachedWorld, agent);
    }

    public int GetStepCount()
    { 
    
        WorldStepCountBehavior wscb = AttachedAgent.World.GetComponent<WorldStepCountBehavior>();
        if (wscb)
            return wscb.StepCount;
        else
            return 0;
    }

    public List<AAgent> GetAgentCollidersAroundPosition(World world, Vector3 pos, float radius, bool includeItself = false)
    {
        List<AAgentCollider> list = GetAgentsAroundPosition<AAgentCollider>(world, pos, radius);
        List<AAgent> agents = new List<AAgent>();
        foreach (AAgentCollider ac in list)
        {
            agents.Add(ac.AttachedAgent);
        }
        if (!includeItself)
        {
            agents.Remove(AttachedAgent);
        }
        
        return agents;
    }

    public List<T> GetAgentsAroundPosition<T>(Vector3 pos, float radius, bool includeItself = false, bool wrap = false) where T : AComponent
    {
        return GetAgentsAroundPosition<T>(AttachedAgent.World, pos, radius, includeItself, wrap);
    }

    private static void AddItems<T>(Vector3 pos, float radius, List<T> l, bool is2D) where T : AComponent
    {
        Type t = typeof(T);
        
        List<AComponent> acs = AriscoSystem.GetAComponents<T>();
        if (acs == null)
            return;

        float rr = radius * radius;
        foreach (AComponent ac in acs)
        {
            if((ac.transform.position-pos).sqrMagnitude <= rr){
                T item = ac.GetComponent(t) as T;
                l.Add(item);
            }
        }
    }

    public List<T> GetAgentsAroundPosition<T>(World world, Vector3 pos, float radius, bool includeItself = false, bool wrap = true) where T : AComponent
    {
        List<T> l = new List<T>();
        
        bool is2D = (GetComponent<Collider2D>() != null);

        AddItems(pos, radius, l, is2D);
        if (!includeItself)
            l.Remove(AttachedAgent.GetComponent<T>());
        
        if (wrap)
        {
            LimitedWorld lw = world.GetComponent<LimitedWorld>();
            if (lw && lw.closed && lw.looped)
            {
                Vector3 size = lw.size;
                Bounds b = new Bounds(world.transform.position + lw.offset, size);
                Vector3 p = pos;
                {
                    if (pos.x + radius > b.max.x)
                    {
                        pos.x -= size.x;
                    } else if (pos.x - radius < b.min.x)
                    {
                        pos.x += size.x;
                    }
                    if (pos.x != p.x)
                    {
                        AddItems(pos, radius, l, is2D);
                        
                        if (!includeItself)
                            l.Remove(AttachedAgent.GetComponent<T>());
                    }
                }
                {
                    pos = p;
                    if (pos.y + radius > b.max.y)
                    {
                        pos.y -= size.y;
                    } else if (pos.y - radius < b.min.y)
                    {
                        pos.y += size.y;
                    }
                    if (pos.y != p.y)
                    {
                        AddItems(pos, radius, l, is2D);
                        
                        if (!includeItself)
                            l.Remove(AttachedAgent.GetComponent<T>());
                    }
                }
                {
                    pos = p;
                    if (pos.z + radius > b.max.z)
                    {
                        pos.z -= size.z;
                    } else if (pos.z - radius < b.min.z)
                    {
                        pos.z += size.z;
                    }
                    if (pos.z != p.z)
                    {
                        AddItems(pos, radius, l, is2D);
                        
                        if (!includeItself)
                            l.Remove(AttachedAgent.GetComponent<T>());
                    }
                }
            }
        }

        return l;
    }



}
