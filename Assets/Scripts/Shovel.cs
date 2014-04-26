using UnityEngine;
using System.Collections;

public class Shovel : Item {

	public override void Init() {
	
	}
	
	public override void Operate(Vector2 dir) {
		xform.localPosition = 1 * xform.right;
		
		var basePos = fx.parent.position;
		
		WorldGen.inst.DigShovel((int)(basePos.x + dir.x + 0.5f),(int)(basePos.y + dir.y + 0.5f));
		
	}
	
	void Update () 
	{
		if (this == Hero.inst.currItem) {
			fx.localPosition = fx.localPosition.EaseTowards (Vector3.zero, 0.1f); 
		}
	}
	
	
	
	
}

