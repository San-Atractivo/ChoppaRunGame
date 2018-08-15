using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utills {

    public static string NumberToCommaText(int value)
    {
        return string.Format("{0:#,##0}", value);
    }

    public static string GetNowDateTime()
    {
        return System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");
    }

    public static string DateTimeToString(System.DateTime time)
    {
        return time.ToString("yyyy/MM/dd HH:mm");
    }
}
