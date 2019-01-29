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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace PeriodTrackerForm
{
    public partial class FormTracker : Form
    {
        public FormTracker()
        {
            InitializeComponent();
            BinaryRead();
        }

        List<Period> MyTracker = new List<Period>();

        //Save method
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Period myPeriod = new Period();
            myPeriod.newDate = dateTimePicker.Value;
            myPeriod.lastDate = dateTimePicker2.Value;
            int indx = MyTracker.Count;
                
            if (indx == 0)
            {
                myPeriod.previousDate = myPeriod.newDate;
                MyTracker.Add(myPeriod);
            }
            else
            {
                myPeriod.previousDate = MyTracker[indx - 1].newDate;
                MyTracker.Add(myPeriod);
            }

            myPeriod.Notes = textBoxNotes.Text;
            DisplayTracker();
        }

        //Update method
        private void Update_btn_Click(object sender, EventArgs e)
        {
            if (listViewTracker.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select a record to update", "Error");
            }
            else
            {
                int indx = listViewTracker.SelectedIndices[0];
                Period updatePeriod = new Period();
                updatePeriod.newDate = dateTimePicker.Value;
                updatePeriod.lastDate = dateTimePicker2.Value;
                updatePeriod.Notes = textBoxNotes.Text;
                MyTracker[indx].newDate = updatePeriod.newDate;
                MyTracker[indx].lastDate = updatePeriod.lastDate;
                MyTracker[indx].Notes = updatePeriod.Notes;
                DisplayTracker();
                Clear();
            }
        }

        //Display method
        public void DisplayTracker()
        {
            listViewTracker.Items.Clear();
            foreach (var perioddate in MyTracker)
            {
                ListViewItem item = new ListViewItem(perioddate.newDate.ToString("dd MMM yyy"));
                item.SubItems.Add(perioddate.CalculateDays() + " days");
                item.SubItems.Add(perioddate.lastDate.ToString("dd MMM yyy"));
                item.SubItems.Add(perioddate.CalculateDuration() + " days");
                item.SubItems.Add(perioddate.Notes);
                listViewTracker.Items.Add(item);
            }
        }

        //Clear method
        public void Clear()
        {
            textBoxNotes.Text = "";
            dateTimePicker.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        
        //BinaryRead method
        public void BinaryRead()
        {
            BinaryReader br;
            try
            {
                br = new BinaryReader(new FileStream("data.bin", FileMode.Open));
            }
            catch (Exception fe)
            {
                MessageBox.Show("File Not Open" + fe.Message);
                return;
            }
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                Period newpr = new Period();
                try
                {
                    string d1 = newpr.previousDate.ToString();
                    d1 = br.ReadString();
                    DateTime d1Date = Convert.ToDateTime(d1);
                    newpr.previousDate = d1Date;

                    string d2 = newpr.newDate.ToString();
                    d2 = br.ReadString();
                    DateTime d2Date = Convert.ToDateTime(d2);
                    newpr.newDate = d2Date;

                    string d3 = newpr.lastDate.ToString();
                    d3 = br.ReadString();
                    DateTime d3Date = Convert.ToDateTime(d3);
                    newpr.lastDate = d3Date;
                    
                    newpr.Notes = br.ReadString();
                    MyTracker.Add(newpr);
                }
                catch (Exception fe)
                {
                    MessageBox.Show("Cannot read file " + fe.Message);
                    break;
                }
            }
            br.Close();
            DisplayTracker();
        }

        //BinaryWrite method
        public void BinaryWrite()
        {
            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(new FileStream("data.bin", FileMode.Create));
            }
            catch (Exception fe)
            {
                MessageBox.Show("File Not Created" + fe.Message);
                return;
            }
            try
            {
                foreach (Period pr in MyTracker)
                {
                    bw.Write(pr.previousDate.ToString());
                    bw.Write(pr.newDate.ToString());
                    bw.Write(pr.lastDate.ToString());
                    bw.Write(pr.Notes);
                }
                //MessageBox.Show("File saved successfully", "Information");
            }
            catch (Exception fe)
            {
                MessageBox.Show("File Not Written" + fe.Message);
            }
            bw.Close();
        }

        //Form Closing method
        private void FormTracker_FormClosing(object sender, FormClosingEventArgs e)
        {
           DialogResult closingProgram = MessageBox.Show("Are you sure you want to exit?", "Warning", MessageBoxButtons.YesNo);
            if (closingProgram == DialogResult.Yes)
            {
                BinaryWrite();
            }
            if (closingProgram == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        //Form Loading method
        private void FormTracker_Load(object sender, EventArgs e)
        {
            listViewTracker.View = View.Details;
            listViewTracker.GridLines = true;
            listViewTracker.Columns.Add("Period Date", 100);
            listViewTracker.Columns.Add("Cycle", 60);
            listViewTracker.Columns.Add("Last Day of Period", 100);
            listViewTracker.Columns.Add("Duration", 60);
            listViewTracker.Columns.Add("Notes", 165);
        }

        //Delete method
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listViewTracker.SelectedIndices.Count == 0)
              {
                  MessageBox.Show("Please select a record to delete", "Error");
              }
              else
              {
                int indx = listViewTracker.SelectedIndices[0];
                DialogResult deleteEntry = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.YesNo);
                if (deleteEntry == DialogResult.Yes)
                {
                    MyTracker.RemoveAt(indx);
                    Clear();
                    DisplayTracker();
                    MessageBox.Show("Item's deleted.", "Information");
                }
                else
                {
                    MessageBox.Show("Deletion cancelled", "Information");
                    DisplayTracker();
                }
            }
        }

        //View list details in textboxes
        private void listViewTracker_MouseClick(object sender, MouseEventArgs e)
        {
            if (listViewTracker.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Please select a record to view", "Error");
            }
            else
            {
                int indx = listViewTracker.SelectedIndices[0];
                dateTimePicker.Value = MyTracker[indx].newDate;
                dateTimePicker2.Value = MyTracker[indx].lastDate;
                textBoxNotes.Text = MyTracker[indx].Notes;
            }
        }

        private void ExporttoPDF_btn_Click(object sender, EventArgs e)
        {
            PdfPTable pdfTable = new PdfPTable(listViewTracker.Columns.Count);
            //pdfTable.DefaultCell.Padding = 1;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;

            //Adding Header row
            foreach (ColumnHeader column in listViewTracker.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.Text));
                pdfTable.AddCell(cell);
            }

            //Adding DataRow
            foreach (ListViewItem itemRow in listViewTracker.Items)
            {
                int i = 0;
                for (i = 0; i < itemRow.SubItems.Count; i++)
                {
                    pdfTable.AddCell(itemRow.SubItems[i].Text);
                }
            }

            //Exporting to PDF
            string folderPath = @"C:/Users/Byanks/Documents/Temp/";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (FileStream stream = new FileStream(folderPath + "DataGridViewExport.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(pdfTable);
                pdfDoc.Close();
                stream.Close();
                MessageBox.Show("Data exported to PDF successfully.");
            }
        }
    }
}
