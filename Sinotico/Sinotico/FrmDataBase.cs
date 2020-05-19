//using System;
//using System.Data;
//using System.Globalization;
//using System.Linq;
//using System.Windows.Forms;

//namespace Sinotico
//    {
//    public partial class FrmDataBase : Form
//        {
//        private ToolTip _toolTip1 = new ToolTip();

//        public FrmDataBase()
//            {
//            InitializeComponent();
//            }

//        private void FrmDataBase_Load(object sender, EventArgs e)
//            {
//            LoadShiftsToList();
//            LoadArticlesToList();
//            }

//        #region Shifts

//        private void LoadShiftsToList()
//            {
//            mtxtFrom.Mask = "90:00";
//            mtxtTo.Mask = "90:00";
//            mtxtFrom.ValidatingType = typeof(DateTime);
//            mtxtFrom.TypeValidationCompleted += new TypeValidationEventHandler(Mtxt_TypeValidationCompleted);
//            mtxtFrom.KeyDown += new KeyEventHandler(Mtxt_KeyDown);
//            mtxtTo.ValidatingType = typeof(DateTime);
//            mtxtTo.TypeValidationCompleted += new TypeValidationEventHandler(Mtxt_TypeValidationCompleted);
//            mtxtTo.KeyDown += new KeyEventHandler(Mtxt_KeyDown);

//            lstShifts.Items.Clear();
//            lstShifts.Columns.Clear();

//            lstShifts.View = View.Details;
//            lstShifts.LabelEdit = true;
//            lstShifts.AllowColumnReorder = true;
//            lstShifts.CheckBoxes = false;
//            lstShifts.FullRowSelect = true;
//            lstShifts.GridLines = true;
//            lstShifts.Sorting = SortOrder.Ascending;

//            var query = (from db in _context.shifts
//                         select db).ToList();

//            lstShifts.Columns.Add("Shift", 50, HorizontalAlignment.Left);
//            lstShifts.Columns.Add("From", 70, HorizontalAlignment.Left);
//            lstShifts.Columns.Add("To", 70, HorizontalAlignment.Left);

//            foreach (var item in query)
//                {
//                ListViewItem item1 = new ListViewItem(item.number);
//                item1.SubItems.Add(item.from_time.ToString());
//                item1.SubItems.Add(item.to_time.ToString());

//                lstShifts.Items.Add(item1);
//                }

//            ImageList imageListSmall = new ImageList();
//            imageListSmall.Images.Add(Properties.Resources.checkmark_30);
//            lstShifts.SmallImageList = imageListSmall;

//            _toolTip1.IsBalloon = true;
//            }

//        void Mtxt_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
//            {
//            var mtxt = (MaskedTextBox)sender;

//            //tests to see if user has entered the correct time format

//            if (!e.IsValidInput)
//                {
//                _toolTip1.ToolTipTitle = "Invalid Date";
//                _toolTip1.Show("The data you supplied must be a valid date in the format HH:mm.", mtxt, 0, -20, 2000);
//                }
//            else
//                {
//                _toolTip1.Hide(mtxt);
//                }
//            }

//        // Hide the tooltip if the user starts typing again before the five-second display limit on the tooltip expires.
//        void Mtxt_KeyDown(object sender, KeyEventArgs e)
//            {
//            var mtxt = (MaskedTextBox)sender;

//            _toolTip1.Hide(mtxt);
//            }

//        bool ValidateTime(string time, string format)
//            {
//            return DateTime.TryParseExact(time, format,
//                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outTime);
//            }

//        private void btnAdd_Click_1(object sender, EventArgs e)
//            {
//            var lstOfShifts = new System.Collections.Generic.List<string>();

//            //tests to see if user has already entered selected shift

//            foreach (ListViewItem lvi in lstShifts.Items)
//                {
//                var item1 = lvi.SubItems[0];

//                lstOfShifts.Add(item1.Text);
//                }

//            var selShift = ndNumber.Value.ToString();

//            if (lstOfShifts.Contains(selShift))
//                {
//                MessageBox.Show("Already inserted shift number.");
//                return;
//                }

//            if (ValidateTime(mtxtFrom.Text, "HH:mmm") && ValidateTime(mtxtTo.Text, "HH:mm"))
//                {
//                DateTime.TryParse(mtxtFrom.Text, out DateTime from);
//                DateTime.TryParse(mtxtTo.Text, out DateTime to);

//                var tbl = new shift
//                    {
//                    number = ndNumber.Value.ToString(),
//                    from_time = new TimeSpan(from.Hour, from.Minute, from.Second),
//                    to_time = new TimeSpan(to.Hour, to.Minute, to.Second)
//                    };

//                _context.shifts.Add(tbl);
//                _context.SaveChanges();

//                LoadShiftsToList();
//                }
//            else
//                {
//                MessageBox.Show("Invalid time format.");
//                }

//            mtxtFrom.Clear();
//            mtxtTo.Clear();
//            }

//        private string _selectedSubItem;

//        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
//            {
//            if (lstShifts.SelectedIndices.Count <= 0)
//                {
//                return;
//                }

//            int intselectedindex = lstShifts.SelectedIndices[0];
//            if (intselectedindex >= 0)
//                {
//                _selectedSubItem = lstShifts.Items[intselectedindex].Text;

//                decimal.TryParse(_selectedSubItem, out decimal d);

//                ndNumber.Value = d;
//                mtxtFrom.Text = lstShifts.Items[intselectedindex].SubItems[1].Text;
//                mtxtTo.Text = lstShifts.Items[intselectedindex].SubItems[2].Text;
//                }
//            }

//        private void btnRemove_Click_1(object sender, EventArgs e)
//            {
//            var deleteQuery = from db in _context.shifts
//                              where db.number == _selectedSubItem
//                              select db;

//            foreach (var item in deleteQuery)
//                {
//                _context.shifts.Remove(item);
//                }

//            var dr = MessageBox.Show("Do you want to remove shift nr " + _selectedSubItem + "?", "Sinotico", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//            if (dr == DialogResult.Yes)
//                {
//                _context.SaveChanges();
//                }

//            LoadShiftsToList();
//            }
//        #endregion Shifts

//        #region Articles

//        private void LoadArticlesToList()
//            {
//            lstArts.Items.Clear();
//            var query = (from db in _context.articles
//                        select db).ToList();

//            foreach (var item in query)
//                {
//                lstArts.Items.Add(item.number);
//                }
//            }

//        private void btnAddArt_Click(object sender, EventArgs e)
//            {
//            if (txtArt.Text == string.Empty) return;

//            var tblArt = new article
//                {
//                number = txtArt.Text
//                };

//            _context.articles.Add(tblArt);
//            _context.SaveChanges();

//            LoadArticlesToList();
//            }

//        private void btnModifyArt_Click(object sender, EventArgs e)
//            {
//            var query = (from db in _context.articles
//                         where db.number == _lastSelectedArt
//                        select db).ToList();

//            foreach (var item in query)
//                {
//                item.number = txtArt.Text;
//                _context.SaveChanges();
//                }

//            LoadArticlesToList();
//            }

//        private void btnRemoveArt_Click(object sender, EventArgs e)
//            {
//            var query = (from db in _context.articles
//                         where db.number == _lastSelectedArt
//                         select db).ToList();

//            foreach (var item in query)
//                {
//                _context.articles.Remove(item);
//                _context.SaveChanges();
//                }

//            LoadArticlesToList();
//            }

//        private string _lastSelectedArt;
//        private void lstArts_SelectedIndexChanged(object sender, EventArgs e)
//            {
//            _lastSelectedArt = lstArts.SelectedItem.ToString();
//            txtArt.Text = _lastSelectedArt;
//            }

//        #endregion Articles

//        #region Machines

//        #endregion Machines

//        #region Finesses

//        #endregion Finesses

//        #region Blocks

//        #endregion Blocks

//        #region Reasons

//        #endregion Reasons
//        }
//    }
