using Caliburn.Micro;
using ExeAttacher.Core.UI;
using ExeAttacher.UI.ViewModels.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ExeAttacher.UI.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IMainViewModel, INotifyDataErrorInfo
    {
        private readonly IDictionary<string, string> validationErrors = new Dictionary<string, string>();
        private readonly IWindowService windowService;
        private bool isDoingAction;
        private string sourceFile;

        public MainViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool CanBrowseFile
        {
            get { return !this.IsDoingAction; }
        }

        public bool CanConvertFile
        {
            get { return !this.IsDoingAction && !this.HasErrors; }
        }

        public bool HasErrors
        {
            get { return this.validationErrors.Any(); }
        }

        public string SourceFile
        {
            get
            {
                return sourceFile;
            }

            set
            {
                this.sourceFile = value;
                this.ValidateSelectedFile();
                this.NotifyOfPropertyChange(() => this.SourceFile);
            }
        }

        public bool IsDoingAction
        {
            get
            {
                return isDoingAction;
            }

            set
            {
                this.isDoingAction = value;
                this.NotifyOfPropertyChange(() => this.IsDoingAction);
                this.NotifyOfPropertyChange(() => this.CanBrowseFile);
                this.NotifyOfPropertyChange(() => this.CanConvertFile);
            }
        }

        public void BrowseFile()
        {
            this.SourceFile = this.windowService.ShowOpenFileDialog();
        }

        public async void ConvertFile()
        {
            this.IsDoingAction = true;

            try
            {
                await Task.Delay(3000);
            }
            catch
            {

            }
            finally
            {
                this.IsDoingAction = false;
            }

            this.SourceFile = this.windowService.ShowOpenFileDialog();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            List<string> error = null;

            if (!string.IsNullOrEmpty(propertyName) && this.validationErrors.ContainsKey(propertyName))
            {
                error = new List<string>
                {
                    this.validationErrors[propertyName]
                };
            }

            return error;
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ValidateSelectedFile()
        {
            string propertyName = nameof(this.SourceFile);

            if (string.IsNullOrEmpty(this.sourceFile))
            {
                if (this.validationErrors.ContainsKey(propertyName))
                {
                    this.validationErrors[propertyName] = "select a file";
                }
                else
                {
                    this.validationErrors.Add(nameof(this.SourceFile), "select a file");
                }
                this.RaiseErrorsChanged(propertyName);
                this.NotifyOfPropertyChange(() => this.CanConvertFile);
            }
            else if (this.validationErrors.ContainsKey(propertyName))
            {
                this.validationErrors.Remove(propertyName);
                this.RaiseErrorsChanged(propertyName);
                this.NotifyOfPropertyChange(() => this.CanConvertFile);
            }
        }
    }
}