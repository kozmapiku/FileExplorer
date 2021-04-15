using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace FileExplorer
{
    class FileExplorerModel
    {
        private Dictionary<string, DirectoryInfo> listedDirectories;
        private List<FileInfo> listedFiles;
        
        public event EventHandler<DirectoryExpandedEventArgs> DirectoryExpanded;
        public event EventHandler<FilesListedEventArgs> FilesListed;

        public FileExplorerModel()
        {
            listedDirectories = new Dictionary<string, DirectoryInfo>();
            listedFiles = new List<FileInfo>();
        }
        public void ListDrive()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                string rootPath = drive.RootDirectory.FullName;
                listedDirectories.Add(rootPath, drive.RootDirectory);
                // TODO: esemény kiváltása, amellyel a modell jelzi az állapota megváltozását
                OnDirectoryExpanded(rootPath, drive.RootDirectory.FullName, drive.RootDirectory.Name);
            }
        }
        public void ExpandDir(string e)
        {
            try
            {
                // Make a reference to a directory.
                DirectoryInfo di = new DirectoryInfo(e);

                // Get a reference to each directory in that directory.
                DirectoryInfo[] diArr = di.GetDirectories();

                // Display the names of the directories.
                foreach (DirectoryInfo dri in diArr)
                {
                    listedDirectories.Add(dri.FullName, dri);
                    Debug.WriteLine(e+" "+dri.FullName+" "+dri.Name);
                    OnDirectoryExpanded(e, dri.FullName, dri.Name);                
                }

                    
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            
        }
        public void ListFiles(string f)
        {
            try
            {
                DirectoryInfo i = listedDirectories[f];
                FileInfo[] files = i.GetFiles();
                listedFiles.Clear();
                foreach (FileInfo fil in files)
                {
                    listedFiles.Add(fil);
                }
                List<File> temp = new List<File>();
                foreach(FileInfo fil in files)
                {
                    temp.Add(new File(fil.Name, fil.Length, fil.CreationTime));
                }

                OnFilesListed(temp);
            }
            catch(Exception exp)
            {
                Debug.WriteLine(exp);
            }
            
        }
        public void Execute(string e)
        {

        }
        private void OnDirectoryExpanded(string expandedDir, string subDirPath, string subDirName)
        {
            if (DirectoryExpanded != null)
            {
                DirectoryExpanded(this,
                new DirectoryExpandedEventArgs(expandedDir, subDirPath, subDirName));
            }
        }
        private void OnFilesListed(List<File> f)
        {
            if (FilesListed != null)
            {
                FilesListed(this,
                new FilesListedEventArgs(f));
            }
        }

    }
    class DirectoryExpandedEventArgs : EventArgs
    {
        public string ExpandedDir { get; private set; }
        public string SubDirPath { get; private set; }
        public string SubDirName { get; private set; }
        public DirectoryExpandedEventArgs(string expandedDir_, string subDirPath_, string subDirName_)
        {
            ExpandedDir = expandedDir_;
            SubDirPath = subDirPath_;
            SubDirName = subDirName_;
        }
    }
    class FilesListedEventArgs : EventArgs
    {
        public List<File> Files { get; private set; }
        public FilesListedEventArgs(List<File> files_)
        {
            Files = files_;
        }
    }
    class File
    {
        public string Name { get; private set; }
        public long Size { get; private set; }
        public DateTime CreationTime { get; private set; }

        public File(string s, long l, DateTime d)
        {
            Name = s;
            Size = l;
            CreationTime = d;
        }
    }
}
