using UnityEngine;
using System.Collections;
using UnityEditor;
[ExecuteInEditMode]
public class ArtificialPosition : MonoBehaviour {

	public Vector3 position;
	public int layer;
	public Vector3 offset;




	void Update()
	{
		if (Application.isPlaying)
			Destroy (this);
		else if (Selection.activeGameObject == this.gameObject) {
			position=offset +(Vector3)transform.position;
			}
		}

}
