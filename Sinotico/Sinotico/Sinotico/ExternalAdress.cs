using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sinotico
    {
    public partial class ExternalAdress : Form
        {
        public ExternalAdress()
            {
            InitializeComponent();
            }

        private void button1_Click(object sender, EventArgs e)
            {
            Properties.Settings.Default.extentAddress = textBox1.Text;
            Properties.Settings.Default.useExternalAdress = _useExternal;

            Properties.Settings.Default.Save();
            }

        private bool _useExternal;
        private void ExternalAdress_Load(object sender, EventArgs e)
            {
            _useExternal = Properties.Settings.Default.useExternalAdress;

            checkBox1.CheckedChanged += delegate
                {
                    _useExternal = checkBox1.Checked;
                    };

            checkBox1.Checked = _useExternal;
            textBox1.Text = Properties.Settings.Default.extentAddress;
            }
        }
    }
