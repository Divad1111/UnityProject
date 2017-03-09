using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;


public static class UICGTools 
{
    public static Transform FindChild(Transform parent, string childName)
    {
        return parent.Find(childName);
    }

    public static void AppendNewLine(StreamWriter sw, int lineCount, int appendSpaceCount = 0)
    {
        for (int i = 0; i < lineCount; ++i)
        {
            sw.Write(System.Environment.NewLine);
        }
        AppendSpace(sw, appendSpaceCount);
    }

    public static void AppendSpace(StreamWriter sw, int spaceCount)
    {
        for (int i = 0; i < spaceCount; ++i)
        {
            sw.Write(" ");
        }
    }


}
