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
    public partial class SplitConfiguration : Form
    {
        List<ComboBox> _options = new List<ComboBox>();
        private static string[] _modes = new string[] { "eff", "qty", "tempStd", "scarti", "rammendi",
                                                       "cquality", "Pulizia Fronture", "Pulizia Ordinaria"};
        private static string[] _displayModes = new string[] { "Efficiency", "Quantity", "Tempo Standard", "Scarti", "Rammendi",
                                                              "Control Quality", "Pulizia Fronture", "Pulizia Ordinaria"};
        public SplitConfiguration()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            _options.Add(cb_tab_1);
            _options.Add(cb_tab_2);
            _options.Add(cb_tab_3);
            _options.Add(cb_tab_4);
            LoadModes();
            foreach (var cb in _options)
                cb.SelectedValueChanged += cb_SelectedValueChanged;
            btnOk.Click += btnOk_Click;
            btnReset.Click += btnReset_Click;
            base.OnLoad(e);
        }
        private void LoadModes()
        {
            foreach(var cb in _options)
            {
                cb.Items.Clear();
                cb.Items.Add("");
                for (var i = 0; i < _displayModes.Length; i++)
                    cb.Items.Add(_displayModes[i]);
            }
        }
        private void cb_SelectedValueChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;

            foreach(var c in _options)
            {
                if (c.Name == cb.Name) continue;
                if (cb.Text == c.Text)
                {
                    cb.Text = cb.Items[0].ToString();
                    return;
                }
            }
        }
        public string[] ThrowSelectedModes(bool forDisplay)
        {
            string[] selectedValues = new string[_modes.Length];
            var position = 0;
            if(!forDisplay)
            foreach (var c in _options)
            {
                for (var i = 0; i < _displayModes.Length; i++)
                {
                        if(c.Text == string.Empty)
                        {
                            selectedValues[position] = string.Empty;
                            position++;
                            break;
                        }
                        if (c.Text == _displayModes[i])
                        {
                            selectedValues[position] = _modes[i];
                            position++;
                            break;
                        }
                }
            }
            else
                foreach (var c in _options)
                {
                    for (var i = 0; i < _displayModes.Length; i++)
                    {
                        if (c.Text == string.Empty)
                        {
                            selectedValues[position] = string.Empty;
                            position++;
                            break;
                        }
                        if (c.Text == _displayModes[i])
                        {
                            selectedValues[position] = _displayModes[i];
                            position++;
                            break;
                        }
                    }
                }
            return selectedValues;
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            var frmSplitScreen = new SplitScreen();
            frmSplitScreen.WindowState = FormWindowState.Maximized;
            var s = this.ThrowSelectedModes(false);
            SplitScreen.selectedModes = s;
            var n = this.ThrowSelectedModes(true);
            SplitScreen._titles = n;
            frmSplitScreen.Show();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var cb in _options)
                cb.SelectedIndex = 0;
        }
    }
}
