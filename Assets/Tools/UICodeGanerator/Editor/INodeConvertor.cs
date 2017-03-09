using System;
using System.Collections.Generic;


namespace UICodeGenerator
{   
    public interface INodeConvertor
    {
        List<Node> ToNodeList(Node rootNode);
        string ToFieldsName(Node node);
        string ToClickEventFunctionName(Node node);
        string ToChangeValueEventFunctionName(Node node);
    }
}
