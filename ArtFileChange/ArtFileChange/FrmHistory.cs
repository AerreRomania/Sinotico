using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtFileChange
    {
    public partial class FrmHistory : Form
        {
        public FrmHistory()
            {
            InitializeComponent();
            }

        private void FrmHistory_Load(object sender, EventArgs e)
            {
            listBox1.DataSource = FrmRename.ListOfHistory;
            }

        private void button1_Click(object sender, EventArgs e)
            {
            Close();
            }
        }
    }
