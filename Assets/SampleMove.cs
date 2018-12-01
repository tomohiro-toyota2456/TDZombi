using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SampleMove : MonoBehaviour {
    float speed = 2;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        // 右・左
        float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");

        // 上・下
        float y = CrossPlatformInputManager.GetAxisRaw("Vertical");

        // 移動する向きを求める
        Vector2 direction = new Vector2(x, y).normalized;

        // 移動する向きとスピードを代入する
        this.gameObject.transform.localPosition = new Vector3(x, y, 0);
    }


}
