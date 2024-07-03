using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderRenamePrevention : AssetPostprocessor
{
    private const string ProtectedFolderName = "advanced-popup-system";
    private const string WarningMessage = "The folder 'advanced-popup-system' cannot be renamed.";

    public void OnPostprocessAssetRename(string oldPath, string newPath)
    {
        if (IsFolder(oldPath) && oldPath.EndsWith(ProtectedFolderName) && !newPath.EndsWith(ProtectedFolderName))
        {
            Debug.LogError(WarningMessage);
            AssetDatabase.RenameAsset(newPath, ProtectedFolderName);
        }
    }

    private static bool IsFolder(string path)
    {
        FileAttributes attr = File.GetAttributes(path);
        return (attr & FileAttributes.Directory) == FileAttributes.Directory;
    }
}