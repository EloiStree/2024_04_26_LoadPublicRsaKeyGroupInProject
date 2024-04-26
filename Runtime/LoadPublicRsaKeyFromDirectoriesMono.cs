using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LoadPublicRsaKeyFromDirectoriesMono : MonoBehaviour
{

    public bool m_loadAtAwake;
    public string m_fileNameContainingPublicKey = "RSA_PUBLIC_XML.txt";
    public Eloi.AbstractMetaAbsolutePathDirectoryMono [] m_directoryContainingRsaPublicKey;
    public Eloi.AbstractMetaAbsolutePathFileMono [] m_fileWithFoldersPathToImportFrom;
    public Eloi.AbstractMetaAbsolutePathFileMono [] m_loadFromGroupInFile;

    public List<string> m_foldersToFetch= new List<string>();
    public List<string> m_filesToFetch = new List<string>();
    public List<string> m_publicKeyRsaFiles = new List<string>();

    public UnityEvent<string> m_findPublicKey;

    public void Awake()
    {
        if (m_loadAtAwake) {
            LoadDirectoryPublicKeyRsaAndBroadcast();
        }
    }

    [ContextMenu("Load Directory Public Key Rsa")]
    public void LoadDirectoryPublicKeyRsaAndBroadcast() {

        List<string> paths = new List<string>();
        foreach (var item in m_directoryContainingRsaPublicKey)
        {
            if (item != null)
            {
                item.CreateDirectory();
                paths.Add(item.GetPath());
            }
        }
        foreach (var item in m_fileWithFoldersPathToImportFrom)
        {
            if (item != null)
            {
                if (!Eloi.E_FileAndFolderUtility.Exists(item))
                    Eloi.E_FileAndFolderUtility.ExportByOverriding(item, "//Add folder path you want to import from your computer");
                string path = item.GetPath();
                if (File.Exists(path)) { 
                    string [] lines = File.ReadAllLines(path);
                    paths.AddRange(lines);
                }
            }
        }
        SetDirectories(paths.ToArray());
        RefreshPublicKeyRSA();
    }

    public void SetDirectories(params string[] folderAbsolutePath)
    {

        m_foldersToFetch.Clear();
        m_foldersToFetch.AddRange(folderAbsolutePath);

    }
    public void ClearDirectories()
    {
        m_foldersToFetch.Clear();
    }

    public void AppendDirectories(params string[] folderAbsolutePath)
    {

        m_foldersToFetch.AddRange(folderAbsolutePath);

    }

    [ContextMenu("Refresh Public Key file")]
    public void RefreshPublicKeyRSA()
    {
        m_filesToFetch.Clear();
        m_publicKeyRsaFiles.Clear();
        for (int i = m_foldersToFetch.Count - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty (m_foldersToFetch[i]) && Directory.Exists(m_foldersToFetch[i]))
            {
                m_filesToFetch.AddRange(Directory.GetFiles(
                    m_foldersToFetch[i], "*", SearchOption.AllDirectories).Where(
                        k => k.EndsWith(m_fileNameContainingPublicKey)
                    ));
            }
        }

        for (int i = 0; i < m_filesToFetch.Count; i++)
        {
            if (!string.IsNullOrEmpty(m_filesToFetch[i]) && File.Exists(m_filesToFetch[i])) {
                m_publicKeyRsaFiles.Add( File.ReadAllText(m_filesToFetch[i]) );
                
            }
        }

        foreach (var item in m_loadFromGroupInFile)
        {
            if (item == null)
                continue;
            string path = item.GetPath();

            if (path == null)
                continue;
            if (!Eloi.E_FileAndFolderUtility.Exists(item))
                Eloi.E_FileAndFolderUtility.ExportByOverriding(item, "//Add here public key split by 切 ");
            ExportImportRsaKeyAsCompressedFile.ImportRsaKey(path, out List<string> keys);
            foreach (var key in keys)
            {
                m_findPublicKey.Invoke(key);
            }
        }

        foreach (var item in m_publicKeyRsaFiles)
        {
            m_findPublicKey.Invoke(item);
        }

    }
}

