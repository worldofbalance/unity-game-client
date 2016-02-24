using UnityEngine;

public class InputExtended {

	private static float timeLastClick;

	private InputExtended() {}

	public static int GetMouseNumClick(int button) {
		if (Input.GetMouseButtonDown(button)) {
			if (timeLastClick == 0) {
				timeLastClick = Time.time;
				return 1;
			} else {
				timeLastClick = 0;
				return 2;
			}
		}
		// Reset
		if (timeLastClick > 0 && Time.time - timeLastClick > 0.5f) {
			timeLastClick = 0;
		}
		
		return 0;
	}
}