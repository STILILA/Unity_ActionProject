using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoSetColliders : MonoBehaviour
{
	public GameMotion motion;

	List<BoxCollider2D> rects;

	void OnEnable() {
		if (!motion) { motion = GetComponentInParent<GameMotion>(); }
		if (motion) {
			if (rects == null) { rects = GetComponents<BoxCollider2D>().ToList(); }
			motion.SetRects(rects, gameObject.layer);
		}
	}



	void OnDisable() {
		if (!motion) { motion = GetComponentInParent<GameMotion>(); }
		if (motion) {
			if (rects == null) { rects = GetComponents<BoxCollider2D>().ToList(); }
			motion.ClearRects(rects, gameObject.layer);
		}
	}


}
