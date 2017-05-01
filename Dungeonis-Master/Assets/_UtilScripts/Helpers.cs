namespace RPG.UtilScripts
{
	using UnityEngine;

	public static class ObjectExtensions
	{
		/// <summary>
		/// Determines whether this instance is null.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>
		///   <c>true</c> if the specified object is null; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsNull(this object obj)
		{
			return (obj == null);
		}
	}

	public static class Functions
	{
		/// <summary>
		/// Clamps the angle.
		/// </summary>
		/// <param name="angle">The angle.</param>
		/// <param name="min">The minimum.</param>
		/// <param name="max">The maximum.</param>
		/// <returns>An angle clamped between 0-360</returns>
		public static float ClampAngle(float angle, float min, float max)
		{
			float newAngle = angle % 360;

			if (newAngle < 0)
				newAngle += 360;

			return Mathf.Clamp(newAngle, min, max);
		}
	}
}