using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportImportRsaKeyAsCompressedFileMono : MonoBehaviour {
    public string m_splitter = "切";
    public Eloi.AbstractMetaAbsolutePathFileMono m_whereToStoreFile;

    public void ExportRsaKey(IEnumerable<string> rsaKeys) {

        if (m_whereToStoreFile == null)
            return;
        ExportImportRsaKeyAsCompressedFile.ExportRsaKey(m_whereToStoreFile.GetPath(), rsaKeys);
    }

    public void ImportRsaKey(out List<string> rsaKeys)
    {
        rsaKeys = new List<string>();
        if (m_whereToStoreFile == null)
            return;
        ExportImportRsaKeyAsCompressedFile.ImportRsaKey(m_whereToStoreFile.GetPath(), out rsaKeys, m_splitter);
    }
  
}



public class ExportImportRsaKeyAsCompressedFile
{
    public string m_splitter = "切";
    Eloi.AbstractMetaAbsolutePathFileMono m_whereToStoreFile;

    public static void ExportRsaKey(string filePathAbsolute, IEnumerable<string> rsaKeys, string splitter = "切")
    {

        if (filePathAbsolute == null) return;
        Directory.CreateDirectory(Path.GetDirectoryName(filePathAbsolute));
        File.WriteAllText(filePathAbsolute, string.Join("\n\n" + splitter + "\n\n", rsaKeys));
    }

    
    public static void ImportRsaKey(string filePathAbsolute, out List<string> rsaKeys, string splitter = "切")
    {
        rsaKeys = new List<string>();
        if (filePathAbsolute == null)
            return;
        if (File.Exists(filePathAbsolute))
        {
            string[] tokens = File.ReadAllText(filePathAbsolute).Split(splitter);
            foreach (var item in tokens)
            {
                string trimmed = item.Trim();
                if (trimmed.Length > 0)
                {
                    rsaKeys.Add(trimmed);
                }
            }
        }
    }
}
