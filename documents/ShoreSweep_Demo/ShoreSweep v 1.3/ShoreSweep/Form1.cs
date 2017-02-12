using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;


namespace ShoreSweep
{
    public partial class Form1 : Form
    {
        // The coordinate watcher.
       


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            

        }

        private void loadListView(String sfileName)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;


            using (var stream = File.OpenRead(sfileName))
            using (var reader = new StreamReader(stream))
            {
                var data = CsvParser.ParseHeadAndTail(reader, ';', '"');

                var header = data.Item1;
                var lines = data.Item2;
                listView1.CheckBoxes = true;


                for (var i = 0; i < header.Count - 1; i++)
                {
                    //Add column header

                    listView1.Columns.Add(header[i], 100);
                }
                listView1.Columns.Add("Assigned To", 100);
                string[] arr = new string[header.Count];
                ListViewItem itm;
                foreach (var line in lines)
                {
                    for (var i = 0; i < header.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(line[i]))
                        {
                            // Console.WriteLine("{0}={1}", header[i], line[i]);
                            //Add items in the listview
                            arr[i] = line[i];
                        }
                    }

                    itm = new ListViewItem(arr);
                    listView1.Items.Add(itm);
                    //   Console.WriteLine();
                }
            }
            Console.ReadLine();








            //initialising a StreamReader type variable and will pass the file location

            StreamReader oStreamReader = new StreamReader(sfileName);

            DataTable oDataTable = null;
            int RowCount = 0;
            string[] ColumnNames = null;
            string[] oStreamDataValues = null;
            //using while loop read the stream data till end
            while (!oStreamReader.EndOfStream)
            {
                String oStreamRowData = oStreamReader.ReadLine().Trim();
                if (oStreamRowData.Length > 0)
                {
                    oStreamDataValues = oStreamRowData.Split(';');
                    //Bcoz the first row contains column names, we will poluate 
                    //the column name by
                    //reading the first row and RowCount-0 will be true only once
                    if (RowCount == 0)
                    {
                        RowCount = 1;
                        ColumnNames = oStreamRowData.Split(';').Where(x => x.Trim() != string.Empty).ToArray();
                        oDataTable = new DataTable();

                        //using foreach looping through all the column names
                        foreach (string csvcolumn in ColumnNames)
                        {
                            DataColumn oDataColumn = new DataColumn(csvcolumn.ToUpper(), typeof(string));

                            //setting the default value of empty.string to newly created column
                            oDataColumn.DefaultValue = string.Empty;

                            //adding the newly created column to the table
                            oDataTable.Columns.Add(oDataColumn);
                        }
                    }
                    else
                    {
                        //creates a new DataRow with the same schema as of the oDataTable            
                        DataRow oDataRow = oDataTable.NewRow();

                        //using foreach looping through all the column names
                        for (int i = 0; i < ColumnNames.Length; i++)
                        {
                            oDataRow[ColumnNames[i]] = oStreamDataValues[i] == null ? string.Empty : oStreamDataValues[i].ToString();
                        }

                        //adding the newly created row with data to the oDataTable       
                        oDataTable.Rows.Add(oDataRow);
                    }
                }
            }
            //close the oStreamReader object
            oStreamReader.Close();
            //release all the resources used by the oStreamReader object
            oStreamReader.Dispose();

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        //private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    String lat,longi;
        //    if (listView1.SelectedItems.Count == 0)
        //        return;

        //    ListViewItem item = listView1.SelectedItems[0];
        //    //fill the text boxes
         
        //    lat = item.SubItems[1].Text;
        //    longi = item.SubItems[2].Text;

        //    ShowOnMap objMap = new ShowOnMap(lat, longi,null);
            
        //    objMap.ShowDialog();
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            ListView.CheckedListViewItemCollection checkedItems =
            listView1.CheckedItems;
            ArrayList str = new ArrayList();
            string[] stringArrayLatlong = new string[listView1.CheckedItems.Count+1];
            string [] stringArrayImageURL= new string[listView1.CheckedItems.Count + 1];

            int iIndex = 0;
            foreach (ListViewItem item in checkedItems)
            {
                str.Add(item.SubItems[1].Text + "," + item.SubItems[2].Text);
                stringArrayLatlong.SetValue(item.SubItems[1].Text + "," + item.SubItems[2].Text, iIndex);
                stringArrayImageURL.SetValue(item.SubItems[13].Text, iIndex);
                iIndex++;
            }
            
            System.IO.File.WriteAllLines(@"LatLong.txt", stringArrayLatlong);
            
            
            ShowOnMap objMap = new ShowOnMap(null, null, stringArrayImageURL);

            objMap.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User objUser = new User();
            objUser.Show();
        }

        private void importTrashoutCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open CSV File";
            theDialog.Filter = "All files|*.*";
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                
                loadListView(theDialog.FileName.ToString());
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddVolunteer objUser = new frmAddVolunteer();
            objUser.Show();
        }
    }
}
