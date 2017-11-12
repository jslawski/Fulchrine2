using UnityEngine;
using System.Collections;

namespace PolarCoordinates {
	public class PolarCoordinate {
		public float radius;
		public float angle;  //In Radians

		private float ConvertAngleTo360(float angle)
		{
			return ((2*Mathf.PI + angle) % (2*Mathf.PI));
		}

		public PolarCoordinate(float newRadius, Vector3 cartesianPoint) {
			radius = newRadius;
			angle = this.ConvertAngleTo360(Mathf.Atan2(cartesianPoint.y, cartesianPoint.x));
		}

		public PolarCoordinate(float newRadius, Vector2 cartesianPoint) {
			radius = newRadius;
			angle = this.ConvertAngleTo360(Mathf.Atan2(cartesianPoint.y, cartesianPoint.x));
		}

		public PolarCoordinate(Vector3 cartesianPoint) {
			radius = 1;
			angle = this.ConvertAngleTo360(Mathf.Atan2(cartesianPoint.y, cartesianPoint.x));
		}

		public PolarCoordinate(Vector2 cartesianPoint) {
			radius = 1;
			angle = this.ConvertAngleTo360(Mathf.Atan2(cartesianPoint.y, cartesianPoint.x));
		}

		public PolarCoordinate(float newRadius, float newAngle) {
			radius = newRadius;
			angle = newAngle;
		}

		public Vector3 PolarToCartesian() {
			return new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
		}

		public static PolarCoordinate CartesianToPolar(Vector3 cart) {
			return new PolarCoordinate(Mathf.Sqrt(Mathf.Pow(cart.x, 2) + Mathf.Pow(cart.y, 2)), cart);
		}
	}
}
