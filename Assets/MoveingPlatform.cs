using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingPlatform : MonoBehaviour
{
    public float startPosX = 0;
    public float startPosY = 0;
    public float endPosX = 0;
    public float endPosY = 0;
    public float speedX = 1;
    public float speedY = 1;
    int dir = 1;

    Dictionary<GameMotion, Transform> passengers = new Dictionary<GameMotion, Transform>(); // 站在上面的物件(物件motion, 原圖層)



    private void Update() {
		if (dir == 1) {
            var pos = transform.localPosition;
            if (pos.x < endPosX || pos.y < endPosY) {
                pos.x += speedX;
                pos.y += speedY;
                transform.localPosition = pos;

                //foreach (var item in passengers) {
                //   // var motion = item.GetComponent<GameMotion>();
                //  //  if (motion && !motion.IsMoving()) {
                //        var itemPos = item.localPosition;
                //        itemPos.x += speedX;
                //        itemPos.y += speedY;
                //        item.localPosition = itemPos;
                //  //  }
                //}
			} else {
                dir = -1;
			}
		} else {
            var pos = transform.localPosition;
            if (pos.x > startPosX || pos.y > startPosY) {
                pos.x -= speedX;
                pos.y -= speedY;
                transform.localPosition = pos;

                //foreach (var item in passengers) {
                //  //  var motion = item.GetComponent<GameMotion>();
                //  //  if (motion && !motion.IsMoving()) {
                //        var itemPos = item.localPosition;
                //        itemPos.x -= speedX;
                //        itemPos.y -= speedY;
                //        item.localPosition = itemPos;
                //   // }

                //}

            }
            else {
                dir = 1;
            }
        }
	}

	private void OnTriggerEnter2D(Collider2D collision) {
        GameMotion item = null;
        if (collision.GetComponent<MotionGetter>()) {
			item = collision.GetComponent<MotionGetter>().motion;
        } 
        
		if (item && item.gameObject.layer != Global.mapLayer && !passengers.ContainsKey(item)) {
            passengers.Add(item, item.transform.parent);
            item.transform.SetParent(transform);
            //passengers.Add(item);
		}
	}
    private void OnTriggerExit2D(Collider2D collision) {
        GameMotion item = null;
        if (collision.GetComponent<MotionGetter>()) {
            item = collision.GetComponent<MotionGetter>().motion;
        }
        

        if (item && item.gameObject.layer != Global.mapLayer && passengers.ContainsKey(item)) {
            item.transform.SetParent(passengers[item]);
            passengers.Remove(item);
        }
    }
}

