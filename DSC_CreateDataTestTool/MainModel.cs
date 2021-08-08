using System.ComponentModel;

namespace DSC_CreateDataTestTool
{
    public class MainModel : INotifyPropertyChanged
    {
        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                }
                this.OnPropertyChanged("IsEnabled");
            }
        }

        private string itemNameInput;
        public string ItemNameInput
        {
            get { return itemNameInput; }
            set
            {
                if (value != itemNameInput)
                {
                    itemNameInput = value;
                }
                this.OnPropertyChanged("ItemNameInput");
            }
        }

        private int itemNameSheet;
        public int ItemNameSheet
        {
            get { return itemNameSheet; }
            set
            {
                if (value != itemNameSheet)
                {
                    itemNameSheet = value;
                }
                this.OnPropertyChanged("ItemNameSheet");
            }
        }

        private string itemJsonInput;
        public string ItemJsonInput
        {
            get { return itemJsonInput; }
            set
            {
                if (value != itemJsonInput)
                {
                    itemJsonInput = value;
                }
                this.OnPropertyChanged("ItemJsonInput");
            }
        }

        private int itemJsonSheet;
        public int ItemJsonSheet
        {
            get { return itemJsonSheet; }
            set
            {
                if (value != itemJsonSheet)
                {
                    itemJsonSheet = value;
                }
                this.OnPropertyChanged("ItemJsonSheet");
            }
        }

        private string output;
        public string Output
        {
            get { return output; }
            set
            {
                if (value != output)
                {
                    output = value;
                }
                this.OnPropertyChanged("Output");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies objects registered to receive this event that a property value has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
