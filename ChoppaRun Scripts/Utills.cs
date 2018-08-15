using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public static void ReflashCommaText(Text UI, int value)
    {
        UI.text = NumberToCommaText(value);
    }

    public static void ReflashCommaText(Text UI, int value, string nextStr)
    {
        UI.text = NumberToCommaText(value) + nextStr;
    }

    public static void ReflashText(Text UI, string str)
    {
        UI.text = str;
    }

    public static void ReflashPersentText(Text UI, float value, string nextStr)
    {
        UI.text = value + nextStr;
    }
}
