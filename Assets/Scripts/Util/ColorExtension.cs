using UnityEngine;

public static class ColorExtension 
{
	public static string ToHex(this Color color)
	{
		return string.Format ("#{0:X2}{1:X2}{2:X2}", (int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
	}
}

