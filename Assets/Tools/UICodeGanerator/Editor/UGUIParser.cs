//#define NGUI

#if UGUI
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UICodeGenerator
{
    class UGUIParser : Parser
    {
        public UGUIParser(GameObject root)
            :base(root)
        {

        }

        protected override bool IsPanel(GameObject go)
        {
            string goName = go.name.ToLower();
        
            if (goName.Contains("panel"))
            {
                return true;
            }
            return false;
        }

        protected override bool IsEmptyObject(GameObject go)
        {
            return false;
        }

        protected override bool IsTemplate(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("template"))
            {   
                return true;
            }
            return false;
        }

        protected override bool IsLabel(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("text") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<Text>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsTextBox(GameObject go)
        {
            if (go.GetComponent<InputField>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsSprite(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("image") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<Image>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsTexture(GameObject go)
        {
            string goName = go.name.ToLower();
            if (goName.Contains("rawimage") &&
               goName.Contains("(") &&
               goName.Contains(")") &&
               go.GetComponent<RawImage>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsButton(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("button") &&
                goName.Contains("(") &&
                goName.Contains(")") )
            {
                return true;
            }
            return false;
        }

        protected override bool IsSlider(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("slider") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<Slider>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsCheckBox(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("toggle") &&
                goName.Contains("(") &&
                goName.Contains(")") &&
                go.GetComponent<Toggle>() != null)
            {
                return true;
            }
            return false;
        }

        protected override bool IsGrid(GameObject go)
        {
            string goName = go.name.ToLower();

            if (go.GetComponent<GridLayoutGroup>() != null &&
                goName.Contains("(") &&
                goName.Contains(")"))
            {
                return true;
            }
            return false;
        }
        protected override bool IsTable(GameObject go)
        {
            string goName = go.name.ToLower();

            //if (go.GetComponent<UITable>() != null &&
            //    goName.Contains("(") &&
            //    goName.Contains(")"))
            //{
            //    return true;
            //}
            return false;
        }

        protected override bool IsGroup(GameObject go)
        {
            string goName = go.name.ToLower();

            if (goName.Contains("group") &&
                goName.Contains("(") &&
                goName.Contains(")"))
            {
                return true;    
            }
            return false;
        }
    }
}
#endif