using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

using System.Threading;
namespace Ambi
{
    public class FileManage : SingletonGameObject<FileManage>
    {
        private string _divisionSplash;
        private string _saveFolder = "LocalFileFolder";
        private string _rootPath;
        private string _rootDirectoryPath = "";
        private void Awake()
        {
            _divisionSplash = Path.DirectorySeparatorChar.ToString();
            _rootPath = Application.dataPath;

        }
        //private static string _targetPath = RootDirectoryPath;
        /// <summary>
        /// 根文件夹路径；注意：尾部以加斜杠或反斜杠，不用再加，直接接文件名即可O(∩_∩)O~
        /// </summary>
        private string RootDirectoryPath(string saveFolder)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _rootPath = Application.persistentDataPath;
                    _divisionSplash = "/";
                    saveFolder = "";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _rootPath = Application.persistentDataPath;
                    _divisionSplash = "/";
                    saveFolder = "";
                    break;
                case RuntimePlatform.OSXEditor:
                    _divisionSplash = "/";
                    break;
                case RuntimePlatform.OSXPlayer:
                    _divisionSplash = "/";
                    break;
                case RuntimePlatform.WindowsEditor:
                    break;
                case RuntimePlatform.WindowsPlayer:
                    break;
                default:
                    _rootDirectoryPath = Application.dataPath + _divisionSplash + saveFolder;
                    break;
            }
            _rootDirectoryPath = _rootPath + _divisionSplash + saveFolder + _divisionSplash;
            return _rootDirectoryPath;

        }
        //创建文件夹
        private void CreatDirectory(string folder)
        {
            _saveFolder =folder;
            if (Directory.Exists(RootDirectoryPath(_saveFolder)))
            {
                return;
            }
            else
            {
                //创建文件夹
                Directory.CreateDirectory(RootDirectoryPath(_saveFolder));

            }

        }
        //创建文件

        private string GetFilePath(string fileName)
        {
            if (!File.Exists(RootDirectoryPath(_saveFolder))) {
                CreatDirectory(_saveFolder);
            }
            string filePath = RootDirectoryPath(_saveFolder) + fileName + ".txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
            return filePath;

        }

        public void DeleteFile(string fileName) {
            string filePath = RootDirectoryPath(_saveFolder) + fileName + ".txt";
            Debug.Log(filePath);
            if (File.Exists(filePath)) {
                StreamWriter streamWriter = new StreamWriter(filePath, false);
                streamWriter.WriteLine(string.Empty);
                streamWriter.Dispose();
            }
        }
        public string  ReadAllFile(string fileName) {
            string filePath = RootDirectoryPath(_saveFolder) + fileName + ".txt";
            if (!File.Exists(filePath)) {
                return "";
            }
            StreamReader streamReader = new StreamReader(filePath, System.Text.Encoding.UTF8);
            string content= streamReader.ReadToEnd();
            streamReader.Dispose();
            return content;
        }
        //写入数据
        /// <summary>
        /// 保存本地文件 每条数据用#分隔开
        /// </summary>
        /// <param name="content">文件的内容</param>
        /// <param name="fileName">文件名字</param>
        public void WriteDate(string content,string fileName)
        {
            string filePath = GetFilePath(fileName);
            StreamWriter streamWriter = new StreamWriter(filePath, true);
            streamWriter.WriteLine("#"+content);
            streamWriter.Dispose();
            streamWriter.Close();
        }

    }
}
