using UnityEngine;
using System.Collections;

///<summery>
/// The class represents Agent's action (behavior)
///
///@author shiva
///
///</summery>
public abstract class ABehavior : AComponent
{
    public int executionOrder;

    ///<summery>
    /// Override it to program the initializing process in the extended class. this will be called soon after the agent object is created.
    ///</summery>
    public virtual void Initialize()
    {
        
    }

    ///<summery>
    ///  Override it to program the beginning process in the extended class. 
    /// This will be called just before the first {@link Agent#step()} is called.
    ///</summery>
    public virtual void Begin()
    {
        
    }

    ///<summery>
    /// Override it to program the step process in the extended class. 
    ///</summery>
    public virtual void Step()
    {
    }

    ///<summery>
    /// Override it to program the commiting process in the extended class.
    /// This method will be called after {@link Agent#step()}. 
    ///</summery>
    public virtual void Commit()
    {

    }

    ///<summery>
    /// Override it to program the ending process  (that will be called after the last {@link Agent#commit()}) in the extended class.
    ///</summery>
    public virtual void End()
    {

    }

    ///<summery>
    /// Override it to program the disposing process in the extended class.
    ///</summery>
    public virtual void Dispose()
    {
        
    }

}
