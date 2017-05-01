namespace RPG.UtilScripts
{
	using UnityEngine;

	public class SpinMe
		: MonoBehaviour
	{
		[SerializeField] private float xRotationsPerMinute = 1f;
		[SerializeField] private float yRotationsPerMinute = 1f;
		[SerializeField] private float zRotationsPerMinute = 1f;

		// Update is called once per frame
		void Update()
		{
			//degrees frame^-1=seconds frame^-1/seconds minute^-1* degrees rotation^-1 * rotation minute^-1
			//degrees^-1=frame^-1minute*degrees rotation^-1 * rotation minute^-1
			//degrees^-1=frame^-1 * degrees
			float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute;
			transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

			float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute;
			transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

			float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute;
			transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
		}
	}
}
