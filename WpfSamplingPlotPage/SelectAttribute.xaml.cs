using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfSamplingPlotPage
{
    /// <summary>
    /// Interaktionslogik für SelectAttribute.xaml
    /// </summary>
    public partial class SelectAttribute : Window
    {
        #region Fields

        private List<string> m_Description = new List<string>();
        private List<string> m_ID = new List<string>();
        private List<List<string>> m_RowList = new List<List<string>>();

        #endregion // Fields
        
        #region Properties

        /// <summary>
        /// Gets the description list.
        /// </summary>
        public List<string> DescriptionList
        {
            get { return m_Description; }
        }

        public List<string> IdList
        {
            get { return m_ID; }
        }

        #endregion // Properties

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectAttribute"/> class.
        /// </summary>
        /// <param name="rowList">The row list.</param>
        public SelectAttribute(List<List<string>> rowList)
        {
            InitializeComponent();

            // Set localization text
            this.Title = WpfSamplingPlotPage.Properties.Resources.SelectAttributeTitle;
            labelSelectAttribute.Content = WpfSamplingPlotPage.Properties.Resources.LabelSelectAttribute;
            labelSeparatorString.Content = WpfSamplingPlotPage.Properties.Resources.LabelSeparatorString;
            labelAssignAttribute.Content = WpfSamplingPlotPage.Properties.Resources.AttributesLabelTextID;

            m_RowList = rowList;

            foreach (string attribute in m_RowList[0])
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = attribute;
                stackPanelSelectAttribute.Children.Add(checkBox);
            }
        }

        #endregion // Construction

        #region Event handlers

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            /*
            m_Description.Clear();
            // Set description strings
            foreach(List<string> row in m_RowList)
            {
                string description = string.Empty;
                int rowInd = 0;
                foreach (UIElement checkBox in stackPanelSelectAttribute.Children)
                {
                    if ((checkBox as CheckBox).IsChecked.Value)
                    {
                        description += row[rowInd] + textBoxSeparatorString.Text;
                    }
                    rowInd++;
                }
                description = description.Substring(0, description.Length - textBoxSeparatorString.Text.Length);
                m_Description.Add(description);
            }
            */
            this.Close();
        }

        private void buttonID_Click(object sender, RoutedEventArgs e)
        {
            m_ID.Clear();
            bool firstRow = true;
            // Set description strings
            foreach (List<string> row in m_RowList)
            {
                string ID = string.Empty;
                int rowInd = 0;
                foreach (UIElement checkBox in stackPanelSelectAttribute.Children)
                {
                    if ((checkBox as CheckBox).IsChecked.Value)
                    {
                        ID += row[rowInd] + textBoxSeparatorString.Text;
                    }
                    rowInd++;
                }
                ID = ID.Substring(0, ID.Length - textBoxSeparatorString.Text.Length);
                m_ID.Add(ID);
                if (firstRow)
                {
                    textBoxID.Text = ID;
                    firstRow = false;
                }
            }
            // Reset checkboxes
            foreach (UIElement checkBox in stackPanelSelectAttribute.Children)
            {
                (checkBox as CheckBox).IsChecked = false;
            }
        }

        private void buttonText_Click(object sender, RoutedEventArgs e)
        {
            m_Description.Clear();
            bool firstRow = true;
            // Set description strings
            foreach (List<string> row in m_RowList)
            {
                string description = string.Empty;
                int rowInd = 0;
                foreach (UIElement checkBox in stackPanelSelectAttribute.Children)
                {
                    if ((checkBox as CheckBox).IsChecked.Value)
                    {
                        description += row[rowInd] + textBoxSeparatorString.Text;
                    }
                    rowInd++;
                }
                description = description.Substring(0, description.Length - textBoxSeparatorString.Text.Length);
                m_Description.Add(description);
                if (firstRow)
                {
                    textBoxText.Text = description;
                    firstRow = false;
                }
            }
            // Reset checkboxes
            foreach (UIElement checkBox in stackPanelSelectAttribute.Children)
            {
                (checkBox as CheckBox).IsChecked = false;
            }
        }

        #endregion // Event handlers

    }
}
