using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace CustomTool
{
    public class AutoAddNameSpace : UnityEditor.AssetModificationProcessor
    {

        /// <summary>
        /// 自动增加命名空间
        /// </summary>
        /// <param name="assetName">创建的cs脚本的meta文件路径</param>
        private static void OnWillCreateAsset(string assetName)
        {
            assetName = assetName.Replace(".meta","");
            if (File.Exists(assetName))
            {
                if (assetName.EndsWith(".cs"))
                {
                    string text = File.ReadAllText(assetName);

                    //Debug.Log(GetClassName(text));
                    File.WriteAllText(assetName, GetNewScriptContext(GetClassName(text)));
                }
            }
            else
            {
                Debug.LogError("File is not found" + assetName);
            }
        }

       
        private static string GetNewScriptContext(string className)
        {
            ScriptBuildHelper script = new ScriptBuildHelper();
            script.WriteUsing();
            script.WriteNameSpace("CustomGame");
            script.WriteClass(className);
            script.WriteFunction("Awake");
            script.WriteFunction("Start");
            return script.GetCurrContext();
        }

        /// <summary>
        /// 截取类名
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        private static string GetClassName(string text)
        {
            //fileUrl = fileUrl.Replace(".cs", "");
            //int classNameLength = fileUrl.Length - (fileUrl.LastIndexOf("/") + 1);
            //string className = fileUrl.Substring(fileUrl.LastIndexOf("/") + 1 , classNameLength);   
            string patterm = "public class ([A-Za-z0-9_]+)\\s*:\\s*MonoBehaviour";
            var match = Regex.Match(text, patterm);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }
    }
}

