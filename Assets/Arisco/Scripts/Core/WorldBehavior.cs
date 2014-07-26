using UnityEngine;
using System.Collections;

///<summery>
///World Behavior
///
///@author shiva
///
///</summery>
public abstract class WorldBehavior : AriscoTools
{
	
	///<summery>
	///Attached World
	///</summery>
	private World attachedWorld;

	public World AttachedWorld {
		get { 
			if (attachedWorld == null)
				attachedWorld = GetComponent<World> ();
			
			return attachedWorld;
		}
		set{ attachedWorld = value;}
	}
	
	///<summery>
	/// Check if the world is started
	///</summery>
	protected bool started;

	public bool Started {
		get { return started;}
		set{ started = value;}
	}

	///<summery>
	/// Check if the world is finished
	///</summery>
	protected bool finished;

	public bool Finished {
		get { return finished;}
		set{ finished = value;}
	}

	///<summery>
	///継承したクラスにおいて、エージェントの実行を確定する処理を記述します。
	///
	///{@link Behavior#step()}実行後に実行されます。 {@link World}の実装クラスによっては、 全ての
	///{@link Behavior}の{@link Behavior#step()}が実行される前に実行される可能性があります。
	///</summery>
	public virtual void Commit ()
	{

	}

	///<summery>
	///継承したクラスにおいて、エージェントを破棄する処理を記述します。
	///</summery>
	public virtual void Dispose ()
	{

	}

	///<summery>
	///継承したクラスにおいて、最後の{@link Behavior#step()}が実行された直後に実行される処理を記述します。
	///</summery>
	public virtual void End ()
	{

	}

	///<summery>
	///継承したクラスにおいて、エージェントが生成された直後に実行される処理を記述します。
	///</summery>
	public virtual void Initialize ()
	{

	}

	///<summery>
	///継承したクラスにおいて、最初の{@link Behavior#step()}が実行される直前に実行される処理を記述します。
	///</summery>
	public virtual void Begin ()
	{

	}

	///<summery>
	///継承したクラスにおいて、毎ステップ実行される処理を記述します。
	///</summery>
	public virtual void Step ()
	{
		
	}

	void Start(){
	}
	
}
