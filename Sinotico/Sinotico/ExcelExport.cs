using System;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sinotico
    {
    class ExcelExport
        {
        private void CopyAlltoClipboard(DataGridView dgv)
            {
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dgv.SelectAll();
            System.Windows.Forms.DataObject dataObj = dgv.GetClipboardContent();

            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
            }

        public void ExportToExcel(DataGridView dgv, string fileName)
            {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Excel Documents (*.xls)|*.xls";
            sfd.FileName = fileName + "_" + DateTime.Now.ToString("yyyyMMdd-ffff");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Copy DataGridView results to clipboard
                CopyAlltoClipboard(dgv);

                object misValue = System.Reflection.Missing.Value;
                Excel.Application xlexcel = new Excel.Application();

                xlexcel.DisplayAlerts = false; // Without this you will get two confirm overwrite prompts
                Excel.Workbook xlWorkBook = xlexcel.Workbooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                //// Format column D as text before pasting results, this was required for my data
                //Excel.Range rng = xlWorkSheet.get_Range("D:D").Cells;
                //rng.NumberFormat = "@";

                // Paste clipboard results to worksheet range
                Excel.Range CR = (Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                //Excel.Range delRng = xlWorkSheet.get_Range("A:A").Cells;
                //delRng.Delete(Type.Missing);
                //xlWorkSheet.get_Range("A1").Select();
                xlWorkSheet.Columns.AutoFit();

                // Save the excel file under the captured location from the SaveFileDialog
                xlWorkBook.SaveAs(sfd.FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlexcel.DisplayAlerts = true;
                xlWorkBook.Close(true, misValue, misValue);
                xlexcel.Quit();

                RelComObject(xlWorkSheet);
                RelComObject(xlWorkBook);
                RelComObject(xlexcel);

                // Clear Clipboard and DataGridView selection
                Clipboard.Clear();
                dgv.ClearSelection();

                // Open the newly saved excel file
                try
                {
                    if (System.IO.File.Exists(sfd.FileName))
                        System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void RelComObject(object obj)
            {
            try
                {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
                }
            catch (Exception ex)
                {
                obj = null;
                MessageBox.Show("Exception Occurred while releasing object " + ex.ToString());
                }
            finally
                {
                GC.Collect();
                }
            }
        }
    }