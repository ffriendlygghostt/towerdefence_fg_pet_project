using UnityEngine;

public static class TimeFormatter
{
    public static string ToTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int secs = totalSeconds % 60;

        if (hours > 0)
            return $"{hours:00}:{minutes:00}:{secs:00}";
        else
            return $"{minutes:00}:{secs:00}";
    }
}
