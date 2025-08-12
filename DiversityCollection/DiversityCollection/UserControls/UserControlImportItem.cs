using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlImportItem : UserControl
    {
        /*
         * an item may relate to a column with fixed values (e.g. relating to a Enum)
         * an item may base on a file field that has to be separated, e.g. coordinates in one field
         *      an item may have an analysis method, e.g. coordiates
         * an item may be composed of several fields in the file, e.g. Identifications
         *      an item may contain a separator between composed fields, e.g. ' ' for Identifications
         * an item may have to be translated, e.g. taxonomic groups
         * an item may be preset
         *      from a text entry
         *      from a selection
         * an item may relate to another item, e.g. Parasite-Host Relations
         * */

        #region Parameter
        
        System.Collections.Generic.List<DiversityCollection.Import_Column> _ImportColumns;
        
        #endregion

        #region Construction
        
        public UserControlImportItem()
        {
            InitializeComponent();
        }

        public UserControlImportItem(DiversityCollection.Import_Column ImportColumn)
        {
            InitializeComponent();
            this._ImportColumns = new List<Import_Column>();
            this._ImportColumns.Add(ImportColumn);
        }

        public UserControlImportItem(System.Collections.Generic.List<DiversityCollection.Import_Column> ImportColumns)
        {
            InitializeComponent();
            this._ImportColumns = ImportColumns;
        }
        
        #endregion

        private void initControl(DiversityCollection.Import_Column ImportColumn)
        {
            this.checkBoxItem.Text = ImportColumn.Column;
        }

        /// <summary>
        /// An item may have an image, e.g. the reference of an identification may be symbolized with a book icon
        /// </summary>
        public System.Drawing.Image ItemImage
        {
            
            set 
            { 
                this.pictureBoxItemImage.Image = value;
                this.pictureBoxItemImage.Visible = true;
            }
        }

    }
}
