using UnityEngine;
using System.Collections;

public class Shovel : Item {

	public override void Operate(Vector2 dir) {
		
	
		var basePos = Hero.inst.xform.position;
		var didDig = WorldGen.inst.DigShovel((int)(basePos.x + dir.x + 0.5f),(int)(basePos.y + dir.y + 0.5f));
		

		fx.localPosition = 0.8f * dir;

		float d = Vector3.Distance (WorldGen.inst.earthCore.xform.position, Hero.inst.xform.position);
		if (d < 15) {
						WorldGen.inst.earthCore.StartDestroy ();
			didDig = true;
				}

		Jukebox.Play(didDig ? "Dig" : "Derp");
		if (didDig) { CameraFX.inst.Shake(0.5f); }


	}
	
	void Update () 
	{
		if (this == Hero.inst.currItem) {
			fx.localPosition = fx.localPosition.EaseTowards (Vector3.zero, 0.25f); 
		}
	}
	
	public void OnDrawGizmos()
	{
		
		if (Hero.inst == null || this != Hero.inst.currItem) return;
		
		Gizmos.color = Color.white;
		var basePos = Hero.inst.xform.position;
		
		Vector3 digPos = new Vector3 ((int)(basePos.x + dir.x + 0.5f), (int)(basePos.y + dir.y + 0.5f), 0);
		Gizmos.DrawWireCube(digPos, new Vector3(1.0f,1.0f,1.0f));
		
		Vector3 digPosRaw = new Vector3 ((basePos.x + dir.x), (basePos.y + dir.y), 0);
		Gizmos.DrawWireSphere (digPosRaw, 0.1f);
	}
	
	
	
}

