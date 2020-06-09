using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sinotico.DatabaseTableClasses;

namespace Sinotico
{
    public partial class MonthScartiRammendi : Form
    {
        private int _selectedRow = 0;
        private ToolTip _infoTT = new ToolTip();

        public MonthScartiRammendi()
        {
            InitializeComponent();
            btnInsert.Click += btnInsert_Click;
            btnDelete.Click += btnDelete_Click;
            btnChange.Click += btnChange_Click;
            CustomizeDataGridView(dgvMonth);
            dgvMonth.SelectionChanged += dgvMonth_SelectionChanged;
        }

        #region Functions
        private DateTime Get_Date()
        {
            return new DateTime(dtpMonth.Value.Year, dtpMonth.Value.Month, 1);
        }
        private int Get_Scarti()
        {
            int.TryParse(tbScarti.Text, out var scarti);
            return scarti;
        }
        private int Get_Rammendi()
        {
            int.TryParse(tbRammendi.Text, out var rammendi);
            return rammendi;
        }
        private int Get_Produtivo()
        {
            int.TryParse(tbProdutivo.Text, out var produtivo);
            return produtivo;
        }
        private double Get_ConsiderateMachines()
        {
            double.TryParse(tbMacConsiderate.Text, out var macConsiderate);
            return macConsiderate;
        }
        private int Get_Finezza()
        {
            int.TryParse(cbFinezza.Text, out var finezza);
            return finezza;
        }
        private int Get_MancanzaLavoro()
        {
            int.TryParse(tbMancanzaLavoro.Text, out var mancanzaLavoro);
            return mancanzaLavoro;
        }
        private int Get_FermataStraordinaria()
        {
            int.TryParse(tbFermataStraiordinaria.Text, out var fermataStraordinaria);
            return fermataStraordinaria;
        }
        private int Get_TcTeep()
        {
            int.TryParse(tbTempoCalendarioTEEP.Text, out var tcTeep);
            return tcTeep;
        }
        private void FilloutFinezzaCombobox()
        {
            cbFinezza.Items.Clear();
            foreach (var finezza in new string[] { "3", "7", "14" })
                cbFinezza.Items.Add(finezza);
            cbFinezza.Text = string.Empty;
        }
        
        private void CustomizeDataGridView(DataGridView dgv)
        {
            dgv.DoubleBufferedDataGridView(true);
            dgv.DataBindingComplete += delegate
            {
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeRows = false;
                dgv.RowHeadersVisible = false;
                dgv.EnableHeadersVisualStyles = false;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.HeaderText = "\n" + col.Name.Substring(0, 1).ToUpper() + col.Name.Remove(0, 1) + "\n";
                    col.DefaultCellStyle.BackColor = Color.White;//Color.FromArgb(255, 255, 203);
                    col.HeaderCell.Style.BackColor = Color.FromArgb(112, 173, 71);//Color.FromArgb(192, 192, 192);    
                    if( col.Name == "FermataStraordinaria")
                    {
                        col.Width = 110;
                    }
                    else if (col.Name == "MancanzaLavoro" || col.Name == "ConsiderateMac" ||
                             col.Name == "CalendarioTEEP")
                    {
                        col.Width = 90;
                    }
                    else if (col.Name == "Finezza")
                    {
                        col.Width = 50;
                    }
                    else
                    {
                        col.Width = 65;
                    }
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    
                    col.HeaderCell.Style.ForeColor = Color.White;
                    col.HeaderCell.Style.Font = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                    col.DefaultCellStyle.ForeColor = Color.FromArgb(70, 70, 70);
                }              
            };
        }
        private void LoadRecords()
        {
            var records = (from r in Tables.TblMonthTrash
                           select r).ToList();
            dgvMonth.DataSource = records;
            dgvMonth.Columns["Id"].Visible = false;
        }

        private bool Insertion()
        {
            if(EmptyFields())
            {
                return false;
            }
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            var existingItem = (from i in Tables.TblMonthTrash
                                where i.Date == Get_Date() && i.Finezza == Get_Finezza()
                                select i).SingleOrDefault();
            if (existingItem != null)
                return false;
            var newItem = new MonthTrash
            {
                Date = Get_Date(),
                Scarti = Get_Scarti(),
                Rammendi = Get_Rammendi(),
                Produtivo = Get_Produtivo(),
                ConsiderateMac = Get_ConsiderateMachines(),
                Finezza = Get_Finezza(),
                MancanzaLavoro = Get_MancanzaLavoro(),
                FermataStraordinaria = Get_FermataStraordinaria(),
                CalendarioTEEP = Get_TcTeep()
            };
            Tables.TblMonthTrash.InsertOnSubmit(newItem);
            FrmHolidays.dc.SubmitChanges();
            LoadRecords();
            return true;
        }
        private bool Deletion()
        {
            if(dgvMonth.Rows.Count <= 0)
            {
                return false;
            }            
            var selectedRowId = dgvMonth.SelectedRows[0].Cells[0].Value.ToString();
            int.TryParse(selectedRowId, out var id);
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            var itemToDelete = (from record in Tables.TblMonthTrash
                               where record.Id == id
                               select record).SingleOrDefault();
            var dialogResult = MessageBox.Show("Are you sure you want to delete selected item?",
                               "Deletion",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Tables.TblMonthTrash.DeleteOnSubmit(itemToDelete);
                FrmHolidays.dc.SubmitChanges();
                LoadRecords();
                return true;
            }
            else
            {
                return true;
            }
        }
        private bool Change()
        {
            if(dgvMonth.DataSource == null ||
               dgvMonth.Rows.Count <= 0 || EmptyFields())
            {
                return false;
            }
            var selectedRowId = dgvMonth.SelectedRows[0].Cells[0].Value.ToString();
            var selectedRowDate = dgvMonth.SelectedRows[0].Cells[1].Value.ToString();
            int.TryParse(selectedRowId, out var id);
            DateTime.TryParse(selectedRowDate, out var date);
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            var itemToChange = (from record in Tables.TblMonthTrash
                                where record.Id == id
                                select record).SingleOrDefault();
            var existingDates = (from record in Tables.TblMonthTrash
                                 where record.Date != date
                                 select record.Date).ToList();
            if (existingDates.Exists(d => d == Get_Date()))
            {
                return false;
            }
            itemToChange.Date = Get_Date();
            itemToChange.Scarti = Get_Scarti();
            itemToChange.Rammendi = Get_Rammendi();
            itemToChange.Produtivo = Get_Produtivo();
            itemToChange.ConsiderateMac = Get_ConsiderateMachines();
            itemToChange.Finezza = Get_Finezza();
            itemToChange.MancanzaLavoro = Get_MancanzaLavoro();
            itemToChange.FermataStraordinaria = Get_FermataStraordinaria();
            itemToChange.CalendarioTEEP = Get_TcTeep();
            var dialogResult = MessageBox.Show("Are you sure you want to edit selected item?",
                               "Change",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                FrmHolidays.dc.SubmitChanges();
                LoadRecords();
                return true;
            }
            else
            {
                return true;
            }            
        }

        private void SetupToolTip(ToolTip info)
        {
            info.SetToolTip(btnChange, "Select record in table and change existing values before you click 'Edit'.");
            info.SetToolTip(btnInsert, "Enter new values in fields before you click 'Insert'.");
            info.SetToolTip(btnDelete, "Select record in table before you click 'Delete'.");
        }
        private bool EmptyFields()
        {
            if (string.IsNullOrEmpty(tbScarti.Text) ||
                string.IsNullOrEmpty(tbRammendi.Text) ||
                string.IsNullOrEmpty(tbProdutivo.Text) ||
                string.IsNullOrEmpty(tbMacConsiderate.Text) ||
                string.IsNullOrEmpty(cbFinezza.Text) ||
                string.IsNullOrEmpty(tbMancanzaLavoro.Text) ||
                string.IsNullOrEmpty(tbFermataStraiordinaria.Text) ||
                string.IsNullOrEmpty(tbTempoCalendarioTEEP.Text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion Functions

        #region Events
        private void dgvMonth_SelectionChanged(object sender, EventArgs args)
        {
            if (dgvMonth.DataSource == null ||
                _selectedRow < 0 ||
                dgvMonth.Rows.Count <= 0)
            {
                return;
            }
            else
            {
                _selectedRow = dgvMonth.CurrentCell.RowIndex;
                DateTime.TryParse(dgvMonth.Rows[_selectedRow].Cells[1].Value.ToString(), out var date);
                dtpMonth.Value = date;
                tbScarti.Text = dgvMonth.Rows[_selectedRow].Cells[2].Value.ToString();
                tbRammendi.Text = dgvMonth.Rows[_selectedRow].Cells[3].Value.ToString();
                tbProdutivo.Text = dgvMonth.Rows[_selectedRow].Cells[4].Value.ToString();
                tbMacConsiderate.Text = dgvMonth.Rows[_selectedRow].Cells[5].Value.ToString();
                cbFinezza.Text = dgvMonth.Rows[_selectedRow].Cells[6].Value.ToString();
                tbMancanzaLavoro.Text = dgvMonth.Rows[_selectedRow].Cells[7].Value.ToString();
                tbFermataStraiordinaria.Text = dgvMonth.Rows[_selectedRow].Cells[8].Value.ToString();
                tbTempoCalendarioTEEP.Text = dgvMonth.Rows[_selectedRow].Cells[9].Value.ToString();
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (!Insertion())
            {
                MessageBox.Show("An error occured while inserting!",
                                "Insertion error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(!Deletion())
            {
                MessageBox.Show("There is nothing to delete!",
                                "Deletion error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        private void btnChange_Click(object sender, EventArgs args)
        {
            if(!Change())
            {
                MessageBox.Show("An error occured while editing!",
                                "Error editing",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            FilloutFinezzaCombobox();
            FrmHolidays.dc = new DataContext(MainWnd.conString);
            dgvMonth.DoubleBufferedDataGridView(true);
            LoadRecords();
            SetupToolTip(_infoTT);            
            base.OnLoad(e);
        }
        #endregion Events
    }
}
