using UnityEngine;
using System.Collections;

public class Shovel : Item {

	public override void Init() {
	
	}
	
	public override void Operate(Vector2 dir) {
		xform.localPosition = 1 * xform.right;
		
		var basePos = Hero.inst.xform.position;
		
		WorldGen.inst.DigShovel((int)(basePos.x + dir.x + 0.5f),(int)(basePos.y + dir.y + 0.5f));
		
	}
	
	void Update () 
	{
		if (this == Hero.inst.currItem) {
			fx.localPosition = fx.localPosition.EaseTowards (Vector3.zero, 0.1f); 
		}
	}
	
//	void OnDrawGizmos()
//	{
//		Gizmos.color = Color.white;
//		
//		Vector3 digPos = new Vector3 ((int)(fx.position.x + dir.x + 0.5f), (int)(fx.position.y + dir.y + 0.5f), 0);
//		Gizmos.DrawWireCube(digPos, new Vector3(1.0f,1.0f,1.0f));
//		
//		Vector3 digPosRaw = new Vector3 ((fx.position.x + dir.x), (fx.position.y + dir.y), 0);
//		Gizmos.DrawWireSphere (digPosRaw, 0.1f);
//	}
	
	
	
}

