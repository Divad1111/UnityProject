//#define NGUI
//#define UGUI

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UICodeGenerator;

class CodeGeneratorWindow:EditorWindow
{
    string filePath                     = Path.Combine(Application.dataPath, "Temp");
    string fileName                     = "";
    string author                       = "Zhangwei";
    ParserType eParserType              = ParserType.UGUI;
    FileGeneratorType eGeneratorType    = FileGeneratorType.UGUICSharp;
    NodeConvertorType eNodeConvertorType= NodeConvertorType.Default;
    EncodeType eEncoder                 = EncodeType.UTF8;
    static GameObject gameObject        = null;

    Generator generator                 = null;


    [MenuItem("Tools/GenerateUIScript... &#g")]
    public static void ShowWindow()
    {      
        EditorWindow.GetWindow(typeof(CodeGeneratorWindow));
        gameObject = Selection.activeObject as GameObject;
    }

    void OnGUI()
    {  
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        gameObject          = (GameObject)EditorGUILayout.ObjectField("GameObject:", gameObject, typeof(GameObject));
        filePath            = EditorGUILayout.TextField("Path:", filePath);
        fileName            = EditorGUILayout.TextField("FileName:", fileName);
        author              = EditorGUILayout.TextField("Author:", author);
        eParserType         = (ParserType)EditorGUILayout.EnumPopup("ParserType:", eParserType);
        eNodeConvertorType  = (NodeConvertorType)EditorGUILayout.EnumPopup("ConvertPolicy:", eNodeConvertorType);
        eGeneratorType      = (FileGeneratorType)EditorGUILayout.EnumPopup("GeneratorType:", eGeneratorType);
        eEncoder            = (EncodeType)EditorGUILayout.EnumPopup("FileEncoding:", eEncoder);      
    
        //set default script file name.
        if (string.IsNullOrEmpty(fileName) && gameObject != null)
        {
            fileName = gameObject.name + ".cs";
        }
        
        //click generate button
        if (GUILayout.Button("Generate"))
        {
            if (gameObject != null)
            {
                // check path
                if (!Directory.Exists(filePath))
                {
                    Debug.LogError("The path of " + filePath + "isn't found.");
                    return;
                }
                if (string.IsNullOrEmpty(fileName))
                {
                    Debug.LogError("File name is empty.");
                    return;
                }                 

                //creating code generator for ui
                generator = CreateGenerator(eParserType, eNodeConvertorType, eGeneratorType);			
				if (generator == null)
				{
					Debug.LogError("generator is null.");
					return;
				}

                var errorCode = generator.GenerateFile(Path.Combine(filePath, fileName), author, GetEncodeing(eEncoder));
                if (errorCode == ErrorCode.CreateFileFailed)
                {
                    EditorUtility.DisplayDialog("Tips", "Creating file is failed.", "Ok");
                }
                else if(errorCode == ErrorCode.GenerateFileFailed)
                {
                    EditorUtility.DisplayDialog("Tips", "Generating file is failed.", "Ok");
                }
                else if (errorCode == ErrorCode.NodeConvertFailed)
                {
                    EditorUtility.DisplayDialog("Tips", "Node convert is failed.", "Ok");
                }
                else if (errorCode == ErrorCode.SameFieldsName)
                {
                    EditorUtility.DisplayDialog("Tips", "Exists same object name.", "Ok");
                }
                else
                {
                    EditorUtility.DisplayDialog("Tips", "generating script is success.", "Ok");
                }

                AssetDatabase.Refresh();   
            }
            else 
            {   
                Debug.LogError("Please assign a parse gameobject.");
            }
        }  

        // show same name object.
        if (generator != null)
        {
            var sameNameObjects = generator.GetSameNameObjects();
            foreach (var sameNameobject in sameNameObjects)
            {
                if (sameNameobject.Value.Count > 1)
                {
                    foreach (var objectName in sameNameobject.Value)
                    {
                        if (GUILayout.Button(objectName.gameObject.name))
                        {
                            EditorGUIUtility.PingObject(objectName.gameObject);
                        }
                    }
                }
            }
        }
    }

    private Generator CreateGenerator( ParserType pType, NodeConvertorType nType, FileGeneratorType gType)
    {
        //creating parser.
        Parser parser = null;
        switch (pType)
        {
		#if NGUI
            case ParserType.NGUI:
                parser = new NGUIParser(gameObject);
                break;
		#endif
		#if UGUI
            case ParserType.UGUI:
                parser = new UGUIParser(gameObject);
                break;
		#endif
            default:
                Debug.LogError("unknown generator type.");
                break;
        }
       
        if (parser == null)
        {
            Debug.LogError("Creating parser is failed.");
            return null;
        }

        //creating node converter
        INodeConvertor nodeConvertor = null;
        switch(nType)
        {
            case NodeConvertorType.Default:
                nodeConvertor = new DefaultNodeConvertor();
                break;
            default:
                Debug.LogError("Type of node converter is unknown.");
                break;
        }

        if (nodeConvertor == null)
        {
            Debug.LogError("Creating node converter is failed.");
            return null;
        }

        //creating file generator
        IFileGenerator fileGenerator = null;
        switch (gType)
        {
            case FileGeneratorType.NGUICSharp:
                fileGenerator = new NGUICSharpFileGenerator();
                break;
            case FileGeneratorType.NGUILua:
                Debug.LogError("no implement.");
                break;
            case FileGeneratorType.UGUICSharp:
                fileGenerator = new UGUICSharpFileGenerator();
                //Debug.LogError("no implement.");
                break;
            case FileGeneratorType.UGUILua:
                Debug.LogError("no implement.");
                break;
            default:
                Debug.LogError("unknown generator type.");
                break;
        }

        if (fileGenerator == null)
        {
            Debug.LogError("Creating file generator is failed.");
            return null;
        }

        return Generator.CreateGenerator(parser, nodeConvertor, fileGenerator);
    }

    public static Encoding GetEncodeing(EncodeType encodeType)
    {
        if (encodeType == EncodeType.ASCII)
            return Encoding.ASCII;
        if (encodeType == EncodeType.UTF8)
            return Encoding.UTF8;
        return Encoding.UTF8;
    }


    #region Test
    void DumpAllNode(Node root)
    {
        if (root == null)
            return;
        //Debug.Log("Parent:" +  root.type.ToString() + "  " + root.gameObject.name);
        if (root.child.Count <= 0)
        {
            Debug.Log("Item:" + root.type.ToString() + "  " + root.gameObject.name);
        }
        else if (root.type != NodeType.Root)
        {
            Debug.Log("Parent:" + root.type.ToString() + "  " + root.gameObject.name);
        }
//         foreach (var item in root.child)
//         {
//             Debug.Log("Item:" + root.type.ToString() + " ---- " + root.gameObject.name);
//             //DumpAllNode(item);
//         }

        foreach (var item in root.child)
        {
            DumpAllNode(item);
        }
    }

    void ClearAllNode(Node root)
    {
        if (root == null)
            return;
        foreach (var item in root.child)
        {
            ClearAllNode(item);
            item.child.Clear();
        }
    }

    #endregion
}
