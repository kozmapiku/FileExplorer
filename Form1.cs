using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class FileExplorerForm : Form
    {
        private FileExplorerModel model;
        private void DirectoryExpanded(object sender, DirectoryExpandedEventArgs e)
        {
            TreeNode[] expandedNode = treeView.Nodes.Find(e.ExpandedDir, true);
            Debug.WriteLine(e.ExpandedDir +"  " +expandedNode.Length);
            //Mindig nullát ad vissza valamiért...

            if (expandedNode.Length == 0) // meghajtót adunk hozzá
            {
                // ...
                treeView.Nodes.Add(e.ExpandedDir).Nodes.Add("");
                
            }
            else // könyvtárat adunk hozzá
            {
                // ...
                Debug.WriteLine("több van");
                //treeView.SelectedNode.Nodes.Add(e.ExpandedDir);
            }
        }
        private void FilesListed(object sender, FilesListedEventArgs e)
        {
            listView.Items.Clear();
            foreach(File f in e.Files)
            {
                string[] row = { f.Name, f.Size.ToString(), f.CreationTime.ToString() };
                var listViewItem = new ListViewItem(row);
                listView.Items.Add(listViewItem);
            }
        }

        public FileExplorerForm()
        {
            InitializeComponent();

            model = new FileExplorerModel();

            model.DirectoryExpanded += new EventHandler<DirectoryExpandedEventArgs>(DirectoryExpanded);
            model.FilesListed += new EventHandler<FilesListedEventArgs>(FilesListed);

        }

        private void FileExplorerForm_Load(object sender, EventArgs e)
        {
            model.ListDrive();
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            //e.Node.Nodes.Clear();
            Debug.WriteLine("kinyitott"+e.Node.Text);
            model.ExpandDir(e.Node.Text);
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            model.ListFiles(e.Node.Text);

        }
    }
}