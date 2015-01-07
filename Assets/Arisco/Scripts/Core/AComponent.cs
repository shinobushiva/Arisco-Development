#define USE_COLLIDER
//#define USE_LIMITEDWORLD
//#define USE_AGENTFIELD
#define USE_BITMASK

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

    public List<T> GetAgentsAroundPosition<T>(Vector3 pos, float radius, bool includeItself = false, bool wrap = true) where T : AComponent
    {
        return GetAgentsAroundPosition<T>(AttachedAgent.World, pos, radius, includeItself, wrap);
    }

    public List<T> GetAgentsAroundPosition<T>(Vector3 pos, float radius, bool includeItself = false) where T : AComponent
    {
        return GetAgentsAroundPosition<T>(AttachedAgent.World, pos, radius, includeItself);
    }

    
    private static Collider[] cols = {};// = this.cols;
    private static Collider2D[] col2ds = {};// = this.col2ds;

    private static void AddItems<T>(Vector3 pos, float radius, List<T> l, bool is2D) where T : AComponent
    {
        Type t = typeof(T);
        if (!AriscoSystem.typeMaskMap.ContainsKey(t))
            return;

        int mask = AriscoSystem.typeMaskMap[t];
        
        float rr = radius * radius;
        List<AComponent> acs = AriscoSystem.GetAComponents<T>();
        foreach (AComponent ac in acs)
        {
            if((ac.transform.position-pos).sqrMagnitude <= rr){
                //if(ac.rigidbody){
                  //  int bit = (int)ac.rigidbody.drag;
                    //if((mask & bit) != 0){
                        T item = ac.GetComponent(t) as T;
                        if (item != null)
                        {
                            l.Add(item);
                        }
                    //}
                //}
            }
        }
    }

    /*
    private static void AddItems2<T>(Vector3 pos, float radius, List<T> l, bool is2D) where T : AComponent
    {
        
        Type t = typeof(T);
        if (!AriscoSystem.typeMaskMap.ContainsKey(t))
            return;

        int mask = AriscoSystem.typeMaskMap[t];

        if (!is2D)
        {
            cols = Physics.OverlapSphere(pos, radius);
        }
        else
        {
            col2ds = Physics2D.OverlapCircleAll(pos, radius);
        }

        if (!is2D)
        {
            if(!AriscoSystem.typeMaskMap.ContainsKey(t))
                return;

            foreach (Collider c in cols)
            {
#if USE_AGENTFIELD
                if(c.rigidbody){
                    AAgentField aaf = AriscoSystem.agentFieldsA[(int)c.rigidbody.mass];
                    if(aaf.map.ContainsKey(t)){
                        T item = aaf.map[t] as T;
                        l.Add(item);
                    }   
                }
#elif USE_BITMASK
                if(c.rigidbody){
                    int bit = (int)c.rigidbody.drag;
                    if((mask & bit) != 0){
                        T item = c.GetComponent(t) as T;
                        if (item != null)
                        {
                            l.Add(item);
                        }
                    }
                }
#elif !USE_BITMASK

                if (item != null)
                {
                    l.Add(item);
                }
#endif

            }
        }
        else
        {
            foreach (Collider2D c in col2ds)
            {
#if USE_AGENTFIELD
                if(c.rigidbody){
                    AAgentField aaf = AriscoSystem.agentFieldsA[(int)c.rigidbody.mass];
                    Type t = typeof(T);
                    if(aaf.map.ContainsKey(t)){
                        T item = aaf.map[t] as T;
                        l.Add(item);
                    }   
                }

#elif USE_BITMASK
                if(c.rigidbody){
                    int bit = (int)c.rigidbody.drag;
                    if((mask & bit) != 0){
                        T item = c.GetComponent(t) as T;
                        if (item != null)
                        {
                            l.Add(item);
                        }
                    }
                }
#elif !USE_BITMASK
                T item = c.GetComponent(t) as T;
                if (item != null)
                {
                    l.Add(item);
                }
#endif
            }
        }

    }
    */

    //private HashSet<Collider> cols = new HashSet<Collider>();
    //private HashSet<Collider2D> col2ds = new HashSet<Collider2D>();

#if USE_COLLIDER && !USE_LIMITEDWORLD

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
#endif

#if USE_COLLIDER && USE_LIMITEDWORLD
    public List<T> GetAgentsAroundPosition<T>(World world, Vector3 pos, float radius, bool includeItself = false, bool wrap = true) where T : AComponent
    {
        List<T> l = new List<T>();
        //HashSet<Collider> cols = this.cols;
        //HashSet<Collider2D> col2ds = this.col2ds;

        bool is2D = (GetComponent<Collider2D>() != null);

        List<AComponent> list = AriscoSystem.Instance.GetAComponents<T>();

        AddItems(pos, radius, l, is2D);

        /*
        {
            if (!is2D)
            {
                Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
                cols.UnionWith(hitColliders);
            } else
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
                col2ds.UnionWith(hitColliders);
            }
        }
        */

        if (wrap)
        {
            LimitedWorld lw = world.GetComponent<LimitedWorld>();
            if (lw && lw.closed)
            {
                Vector3 size = lw.size;
                Bounds b = new Bounds(world.transform.position + lw.offset, size);
            
                Vector3 p = pos;
                {
                    if (pos.x + 1 > b.max.x)
                    {
                        pos.x -= size.x;
                    } else if (pos.x - 1 < b.min.x)
                    {
                        pos.x += size.x;
                    }
                    if (pos.x != p.x)
                    {
                        AddItems(pos, radius, l, is2D);
                        /*
                        if (!is2D)
                        {
                            Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
                            cols.UnionWith(hitColliders);
                        } else
                        {
                            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
                            col2ds.UnionWith(hitColliders);
                        }
                        */
                    }
                }
                {
                    pos = p;
                    if (pos.y + 1 > b.max.y)
                    {
                        pos.y -= size.y;
                    } else if (pos.y - 1 < b.min.y)
                    {
                        pos.y += size.y;
                    }
                    if (pos.y != p.y)
                    {
                        AddItems(pos, radius, l, is2D);
                        /*
                        if (!is2D)
                        {
                            Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
                            cols.UnionWith(hitColliders);
                        } else
                        {
                            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
                            col2ds.UnionWith(hitColliders);
                        }
                        */
                    }
                }
                {
                    pos = p;
                    if (pos.z + 1 > b.max.z)
                    {
                        pos.z -= size.z;
                    } else if (pos.z - 1 < b.min.z)
                    {
                        pos.z += size.z;
                    }
                    if (pos.z != p.z)
                    {
                        AddItems(pos, radius, l, is2D);
                        /*
                        if (!is2D)
                        {
                            Collider[] hitColliders = Physics.OverlapSphere(pos, radius);
                            cols.UnionWith(hitColliders);
                        } else
                        {
                            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(pos, radius);
                            col2ds.UnionWith(hitColliders);
                        }
                        */
                    }
                }
            }
        }

        if (!is2D)
        {
            if (cols.Count > list.Count)
            {
                foreach (T item in list)
                {
                    if (cols.Contains(item.collider))
                    {
                        l.Add(item);
                    }
                }
            } else
            {
                foreach (Collider c in cols)
                {
                    T item = c.GetComponent<T>();
                    if (list.Contains(item))
                    {
                        l.Add(item);
                    }
                }
            }
        } else
        {
            if (col2ds.Count > list.Count)
            {
                foreach (T item in list)
                {
                    if (col2ds.Contains(item.collider2D))
                    {
                        l.Add(item);
                    }
                }
            } else
            {
                foreach (Collider2D c in col2ds)
                {
                    T item = c.GetComponent<T>();
                    if (list.Contains(item))
                    {
                        l.Add(item);
                    }
                }
            }
        }

        if (!includeItself)
        {
            l.Remove(AttachedAgent.GetComponent<T>());
        }

        cols.Clear();
        col2ds.Clear();
        return l;
    }
#endif

#if !USE_COLLIDER
    public List<T> GetAgentsAroundPosition<T>(World world, Vector3 pos, float radius, bool includeItself = false, bool wrap = true) where T : AComponent
    {
        List<T> l = new List<T>();
        
        bool is2D = (GetComponent<Collider2D>() != null);
        LimitedWorld lw = world.GetComponent<LimitedWorld>();

        Vector3 p = pos;
        Vector3 pos1 = pos;
        Vector3 pos2 = pos;
        Vector3 pos3 = pos;

        if (lw && lw.closed)
        {
            Vector3 size = lw.size;
            Bounds b = new Bounds(world.transform.position + lw.offset, size);

            {
                pos = p;
                if (pos.x + 1 > b.max.x)
                {
                    pos.x -= size.x;
                } else if (pos.x - 1 < b.min.x)
                {
                    pos.x += size.x;
                }
                pos1 = pos;
            }
            {
                pos = p;
                if (pos.y + 1 > b.max.y)
                {
                    pos.y -= size.y;
                } else if (pos.y - 1 < b.min.y)
                {
                    pos.y += size.y;
                }
                pos2 = pos;
            }
            {
                pos = p;
                if (pos.z + 1 > b.max.z)
                {
                    pos.z -= size.z;
                } else if (pos.z - 1 < b.min.z)
                {
                    pos.z += size.z;
                }
                pos3 = pos;
            }
        }
        
        List<T> list = AriscoSystem.Instance.GetAComponents<T>();

        foreach (T item in list)
        {
            {
                if (Vector3.Distance(item.transform.position, pos) <= radius)
                {
                    l.Add(item);
                }
            }

            if (lw && lw.closed)
            {
                if (pos1.x != p.x)
                {
                    if (Vector3.Distance(item.transform.position, pos1) <= radius)
                    {
                        l.Add(item);
                    }
                }
            
                if (pos2.y != p.y)
                {
                    if (Vector3.Distance(item.transform.position, pos2) <= radius)
                    {
                        l.Add(item);
                    }
                }
            }
            if (pos3.z != p.z)
            {
                if (Vector3.Distance(item.transform.position, pos3) <= radius)
                {
                    l.Add(item);
                }
            }
        }
        
        if (!includeItself)
        {
            l.Remove(AttachedAgent.GetComponent<T>());
        }
        
        return l;
    }
#endif

}
