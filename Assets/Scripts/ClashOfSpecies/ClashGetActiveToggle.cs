using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public static class ClashGetActiveToggle {
	public static Toggle GetActiveToggle(this ToggleGroup tg) {
		return tg.ActiveToggles ().FirstOrDefault();
	}
}
