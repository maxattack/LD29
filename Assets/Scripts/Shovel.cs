using UnityEngine;
using System.Collections;

public class Shovel : Item {

	public override void Operate(Vector2 dir) {
		
	
		xform.localPosition = 1 * xform.right;
		
		var basePos = Hero.inst.xform.position;
		
		var didDig = WorldGen.inst.DigShovel((int)(basePos.x + dir.x + 0.5f),(int)(basePos.y + dir.y + 0.5f));
		
		Jukebox.Play(didDig ? "Dig" : "Derp");
		
	}
	
	void Update () 
	{
		if (this == Hero.inst.currItem) {
			fx.localPosition = fx.localPosition.EaseTowards (Vector3.zero, 0.1f); 
		}
	}
	
	public override void OnDrawGizmosEquipped()
	{
		
		Gizmos.color = Color.white;
		var basePos = Hero.inst.xform.position;
		
		Vector3 digPos = new Vector3 ((int)(basePos.x + dir.x + 0.5f), (int)(basePos.y + dir.y + 0.5f), 0);
		Gizmos.DrawWireCube(digPos, new Vector3(1.0f,1.0f,1.0f));
		
		Vector3 digPosRaw = new Vector3 ((basePos.x + dir.x), (basePos.y + dir.y), 0);
		Gizmos.DrawWireSphere (digPosRaw, 0.1f);
	}
	
	
	
}

