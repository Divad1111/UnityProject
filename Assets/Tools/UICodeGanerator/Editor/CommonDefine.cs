using System;
using System.Collections.Generic;
using System.Linq;


namespace UICodeGenerator
{
    public enum ParserType
    {
        NGUI,
        UGUI
    }

    public enum FileGeneratorType
    {
        NGUILua,
        NGUICSharp,
        UGUILua,
        UGUICSharp
    }

    public enum NodeConvertorType
    {
        Default
    }

    public enum ErrorCode
    {
        None,
        CreateFileFailed,
        NodeConvertFailed,
        GenerateFileFailed,
        SameFieldsName
    }

    public enum EncodeType
    {
        UTF8,
        ASCII
    }
}
