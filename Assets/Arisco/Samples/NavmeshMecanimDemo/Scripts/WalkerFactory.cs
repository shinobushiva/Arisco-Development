using UnityEngine;
using System.Collections;

public class WalkerFactory : SingletonMonoBehaviour<WalkerFactory>
{
	
	//public int number = 1;
	public AAgent[] walkers;
	public bool useOne = false;
	public int walkerNum = 0;
	
	public AAgent GetAWalker ()
	{
		if (useOne) {
			return walkers [walkerNum];
		} else {
			if(walkerNum > 1){
				return walkers [Random.Range (0, walkerNum)];
			}else{
				return walkers [Random.Range (0, walkers.Length)];
			}
		}
	}
}
