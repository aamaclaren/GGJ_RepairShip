using UnityEngine;
using System.Collections;

// [ExecuteInEditMode]
public class FollowUV : MonoBehaviour {

	public float parralax = 2f;
	public Vector2 UVScroll;
	Vector2 scrollOffset;
	public Vector2 initialOffset;
	public float stretch = 1;

	Material m;

	private void Start() {
		m = GetComponent<MeshRenderer>().material;
	}

	void Update () {

		Vector2 offset = m.mainTextureOffset;

		m.mainTextureScale = new Vector2(1, stretch);

		offset.x = transform.position.x / transform.localScale.x / parralax;
		offset.y = transform.position.z / transform.localScale.z / parralax;

		scrollOffset += UVScroll;

		m.mainTextureOffset = offset + initialOffset + scrollOffset;

	}

	private void OnDestroy() {
		if (Application.isEditor) {
			DestroyImmediate(m);
		} else {
			Destroy(m);
		}
	}

}
