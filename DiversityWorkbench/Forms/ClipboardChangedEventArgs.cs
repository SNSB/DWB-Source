using System;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    internal class ClipboardChangedEventArgs : EventArgs
    {
        public readonly IDataObject DataObject;

        public ClipboardChangedEventArgs(IDataObject dataObject)
        {
            DataObject = dataObject;
        }
    }
}