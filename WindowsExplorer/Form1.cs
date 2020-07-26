using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WindowsExplorer
{
    public partial class Form1 : Form
    {
        string SelectedDrive, SelectedPath;
        public Form1()
        {
            InitializeComponent();

                                                            //we need to get drives names 
            getDrives(); 
            comboBox1.Text = "C:\\";

                                                            //fill the combobox of the type of icons view
            comboView(); 
            comboBox2.Text = "Details";
        }
        void comboView()

        { 
            comboBox2.Items.Add("LargeIcon"); 
            comboBox2.Items.Add("Details"); 
            comboBox2.Items.Add("SmallIcon");
            comboBox2.Items.Add("List"); 
            comboBox2.Items.Add("Tile"); 
        }

                                                           //get all drives on the pc and put it into the combobox
        void getDrives(){
            foreach (string drive in Directory.GetLogicalDrives()) 
            {
                comboBox1.Items.Add(drive); 
            }
        }

                                                           //evry folder of the selected drive will appear as a loop 
        void getFolders() 
        {
            SelectedDrive = comboBox1.Text; 
            DirectoryInfo Dir = new DirectoryInfo(SelectedDrive);
            treeView1.Nodes.Clear();
            foreach (DirectoryInfo Folder in Dir.GetDirectories())
            { 
                treeView1.Nodes.Add("", Folder.Name, 0, 0); 
            } 
        }

        //
        void getFiles(string strPath)
        {
            ListViewItem lvi;
            DirectoryInfo Dir = new DirectoryInfo( /*this is the name of the drive */ SelectedDrive + /*this is the path of the drive*/ strPath);
            listView1.Items.Clear(); 
            try
            { 
                foreach (FileInfo files in Dir.GetFiles())
                { 
                    lvi = listView1.Items.Add(files.Name, 1);
                    double FileLength = files.Length;
                    lvi.SubItems.Add(Math.Round(FileLength /1000, 3) + " KB");
                    lvi.SubItems.Add(files.LastAccessTime.ToString());
                    lvi.SubItems.Add(files.Extension);
                    lvi.SubItems.Add(files.CreationTime.ToString());
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getFolders();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedPath = e.Node.FullPath;
            /*send the parameter to the function getfiles*/ getFiles(SelectedPath);
            TreeNode node;
            DirectoryInfo Dir = new DirectoryInfo(/*this is the name of the drive */ SelectedDrive + /*this is the path of the drive*/ SelectedPath);
            try 
            { 
                foreach (DirectoryInfo folder in Dir.GetDirectories())
                {
                    node = new TreeNode(folder.Name, 0, 0);
                    e.Node.Nodes.Add(node);
                } 
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
            }
            SelectedPath = SelectedDrive + SelectedPath;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
                if (comboBox2.Text == "LargeIcon")
                    listView1.View = View.LargeIcon;
                else if (comboBox2.Text == "Details")
                    listView1.View = View.Details;
                else if (comboBox2.Text == "SmallIcon")
                    listView1.View = View.SmallIcon;
                else if (comboBox2.Text == "List")
                    listView1.View = View.List;
                else listView1.View = View.Tile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            {
                Process.Start(SelectedPath + "\\" + listView1.FocusedItem.Text);
            } 
            catch (Exception ex) { MessageBox.Show(ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void treeView1_Enter(object sender, EventArgs e)
        {
            button1.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(SelectedPath + "\\" + textBox1.Text)) 
                MessageBox.Show("Exists!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
