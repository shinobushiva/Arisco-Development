using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arisco.Core;

[RequireComponent(typeof(World))]
public class LimitedWorld : WorldBehavior
{
	
		public Vector3 offset;
		public Vector3 size;
		public bool closed = true;
		public bool grid = false;
		public bool adjustMainCamera = false;

		public class LimitedWorldEventAdapter : WorldEventAdapter
		{
				public override void Initialized (World world)
				{
						world.GetComponent<LimitedWorld> ().Adjust ();
				}
		}

		public Bounds Bound {
				get {
						return new Bounds (transform.position + offset, size);
				}
		}

		public void Adjust ()
		{

				if (!enabled)
						return;
		
				if (closed) {
						Bounds b = Bound;
						List<AAgent> agents = AttachedWorld.AllAgents;
						foreach (AAgent a in agents) {
								if (!b.Contains (a.transform.position)) {
										Vector3 pos = a.transform.position;
										if (pos.x > b.max.x) {
												pos.x -= size.x;
										} else if (pos.x < b.min.x) {
												pos.x += size.x;
										}
										if (pos.y > b.max.y) {
												pos.y -= size.y;
										} else if (pos.y < b.min.y) {
												pos.y += size.y;
										}
										if (pos.z > b.max.z) {
												pos.z -= size.z;
										} else if (pos.z < b.min.z) {
												pos.z += size.z;
										}
										a.transform.position = pos;
								}
						}
				}
		
				if (grid) {
						List<AAgent> agents = AttachedWorld.AllAgents;
						foreach (AAgent a in agents) {
								Vector3 pos = a.transform.position;
								a.transform.position = ToGrid (pos);
						}
				}
		}

		LimitedWorldEventAdapter limitedWorldListener;

		public override void Initialize ()
		{
				limitedWorldListener = new LimitedWorldEventAdapter ();
				print ("Add World Listener:" + limitedWorldListener);
				AttachedWorld.AddWorldEventListener (limitedWorldListener);
		}

		public override void Commit ()
		{
				Adjust ();
		}

		public override void Dispose ()
		{
				AttachedWorld.RemoveWorldEventListener (limitedWorldListener);
		}

		void Start ()
		{
				if (adjustMainCamera) {
						Camera.main.transform.position = new Vector3 (transform.position.x, Mathf.Max (size.x, size.y, size.z), transform.position.z);
				}
		}

		void OnDrawGizmos ()
		{
				if (enabled) {
						Gizmos.color = Color.white;
						Gizmos.DrawWireCube (transform.position + offset, size);
				}
		}
}
