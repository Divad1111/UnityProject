using System.Collections.Generic;
using System.IO;


namespace UICodeGenerator
{
    public interface IFileGenerator
    {
        void OnBeginWriteFileHeader(StreamWriter sw, string fileName, string author);
        void OnEndWriteFileHeader(StreamWriter sw, string fileName, string author);
        void OnBeginWriteClass(StreamWriter sw, string fileName);
        void OnEndWriteClass(StreamWriter sw, string fileName);
        void OnWriteFields(StreamWriter sw, Node node, string fieldsName);
        void OnBeginWriteInitFunction(StreamWriter sw);
        void OnWriteInitFunction(StreamWriter sw, Node node, string fieldsName, Node parentNode, string parentFieldsName);
        void OnBeginWriteEventBind(StreamWriter sw);
        void OnWriteEventBind(StreamWriter sw, Node node, string fieldsName, string functionName);
        void OnEndWriteInitFunction(StreamWriter sw);
        void OnWriteOtherFunction(StreamWriter sw);
        void OnWriteEventFunction(StreamWriter sw, Node node, string functionName);
        // function signature:Transform Func (Transform parent, string childName)
        string GetFunctionNameOfFindChid();
    }
}
