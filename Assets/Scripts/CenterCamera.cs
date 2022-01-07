using UnityEngine;

public class CenterCamera : MonoBehaviour
{
	public GameObject obj;
	public float speed;
	void Update()
	{
		float interpolation = speed * Time.deltaTime;
		Vector3 position = this.transform.position;
		position.y = Mathf.Lerp(this.transform.position.y, obj.transform.position.y, interpolation);
		position.x = Mathf.Lerp(this.transform.position.x, obj.transform.position.x, interpolation);
		this.transform.position = position;
	}
}
