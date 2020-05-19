using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class RepMachineCleaners : Form
        {
        public static string _puliziaType = string.Empty;

        public void SetPuliziaType(string type)
        {
            _puliziaType = type;
        }
        public RepMachineCleaners()
            {
            InitializeComponent();
            }

        private DataTable _dataTable = new DataTable();

        private void LoadData()
            {
            _dataTable = new DataTable();

            int.TryParse(MainWnd._mouseOverMachineNumber, out var machine);
            if (machine <= 0 || machine > 210)
                {
                MessageBox.Show("Invalid machine number.", "Sinotico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                }

            using (var con = new SqlConnection(MainWnd.conString))
                {
                var cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[getcleanersonmachine]";
                cmd.Connection = con;

                cmd.Parameters.Add("@startdate", SqlDbType.DateTime).Value = MainWnd.Get_from_date();
                cmd.Parameters.Add("@enddate", SqlDbType.DateTime).Value = MainWnd.Get_to_date();
                cmd.Parameters.Add("@machine", SqlDbType.Int).Value = machine;
                cmd.Parameters.Add("@shiftArray", SqlDbType.NVarChar).Value = MainWnd.Get_shift_array().ToString();
                cmd.Parameters.Add("@puliziaType", SqlDbType.NVarChar).Value = _puliziaType;

                con.Open();
                var dr = cmd.ExecuteReader();
                _dataTable.Load(dr);
                con.Close();
                dr.Close();
                }

            dgvReport.DataSource = _dataTable;

            dgvReport.Columns[0].Visible = false;
            }

        private void RepMachineCleaners_Load(object sender, EventArgs e)
            {
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeColumns = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.AllowUserToOrderColumns = false;
            dgvReport.ReadOnly = true;
       
            dgvReport.DoubleBufferedDataGridView(true);

            lblMac.Text = "Macchina: " + MainWnd._mouseOverMachineNumber.ToString();

            LoadData();
            }
        }
    }
