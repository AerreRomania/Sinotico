using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace Sinotico
    {
    public static class ExtensionMethods
        {
        /// <summary>
        /// Doubles the buffered data grid view.
        /// </summary>
        /// <param name="dgv">The DGV.</param>
        /// <param name="setting">if set to <c>true</c> [setting].</param>
        public static void DoubleBufferedDataGridView(this DataGridView dgv, bool setting)
            {
            var dgvType = dgv.GetType();
            var pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
            }

        /// <summary>
        /// Doubles the buffered panel.
        /// </summary>
        /// <param name="pn">The pn.</param>
        /// <param name="setting">if set to <c>true</c> [setting].</param>
        public static void DoubleBufferedPanel(this Panel pn, bool setting)
            {
            var pnType = pn.GetType();
            var pi = pnType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(pn, setting, null);
            }

        /// <summary>
        /// Doubles the buffered form.
        /// </summary>
        /// <param name="frm">The FRM.</param>
        /// <param name="setting">if set to <c>true</c> [setting].</param>
        public static void DoubleBufferedForm(this Form frm, bool setting)
            {
            var frmType = frm.GetType();
            var pi = frmType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (pi != null) pi.SetValue(frm, setting, null);
            }

        /// <summary>
        /// Doubles the buffered form.
        /// </summary>
        /// <param name="btn">The FRM.</param>
        /// <param name="setting">if set to <c>true</c> [setting].</param>
        public static void DoubleBufferedButton(this Button btn, bool setting)
            {
            var frmType = btn.GetType();
            var pi = frmType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(btn, setting, null);
            }

        /// <summary>
        /// Clones the specified control to clone.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controlToClone">The control to clone.</param>
        /// <returns></returns>
        public static T Clone<T>(this T controlToClone)
        where T : Control
            {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (var propInfo in controlProperties)
            {
                if (!propInfo.CanWrite) continue;
                if (propInfo.Name != "WindowTarget")
                    propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
            }

            return instance;
            }

        public static int GetSequences(this IEnumerable<int> sequence, int other)
            {
            int? first = null;

            foreach (var item in sequence)
                {
                if (first == null)
                    {
                    first = item;
                    }
                else if (first.Value != item)
                    {
                    return other;
                    }
                }

            return first ?? other;
            }

        public static void CopyControl(Control sourceControl, Control targetControl)
            {
            // make sure these are the same
            if (sourceControl.GetType() != targetControl.GetType())
                {
                throw new Exception("Incorrect control types");
                }

            foreach (PropertyInfo sourceProperty in sourceControl.GetType().GetProperties())
                {
                object newValue = sourceProperty.GetValue(sourceControl, null);

                MethodInfo mi = sourceProperty.GetSetMethod(true);
                if (mi != null)
                    {
                    sourceProperty.SetValue(targetControl, newValue, null);
                    }
                }
            }

        public static DataTable Table(this BindingSource bs)
            {
            var bsFirst = bs;

            while (bsFirst.DataSource is BindingSource)
                bsFirst = (BindingSource)bsFirst.DataSource;

            DataTable dt = new DataTable();
            dt.Clear();

            if (bsFirst.DataSource is DataSet)
                dt = ((DataSet)bsFirst.DataSource).Tables[bsFirst.DataMember];
            else if (bsFirst.DataSource is DataTable)
                dt = (DataTable)bsFirst.DataSource;
            else
                return null;
            
            if (bsFirst != bs)
                {
                if (dt.DataSet == null) return null;
                dt = dt.DataSet.Relations[bs.DataMember].ChildTable;
                }

            return dt;
            }
        }
    }
