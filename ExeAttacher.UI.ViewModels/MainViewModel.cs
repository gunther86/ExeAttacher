using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using ExeAttacher.Core.Constants;
using ExeAttacher.Core.Services;
using ExeAttacher.Core.UI;
using ExeAttacher.UI.Resources;
using ExeAttacher.UI.ViewModels.Interfaces;

namespace ExeAttacher.UI.ViewModels
{
    public class MainViewModel : PropertyChangedBase, IMainViewModel, INotifyDataErrorInfo
    {
        private readonly IDictionary<string, string> validationErrors = new Dictionary<string, string>();
        private readonly IWindowService windowService;
        private readonly IAttachService attachService;
        private bool isDoingAction;
        private string sourceFile;

        public MainViewModel(IWindowService windowService, IAttachService attachService)
        {
            this.windowService = windowService;
            this.attachService = attachService;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool CanBrowseFile => !this.IsDoingAction;

        public bool CanConvertFile => !this.IsDoingAction && !this.HasErrors;

        public bool HasErrors => this.validationErrors.Any();

        public string SourceFile
        {
            get
            {
                return this.sourceFile;
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
                return this.isDoingAction;
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
            if(this.IsExeFile())
            {
                await this.ExecuteActionWithTryCatch(this.AttachExeAction);
            }
            else if(this.IsAttachedFile())
            {
                await this.ExecuteActionWithTryCatch(this.RevertExeAction);
            }
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
            if (this.ErrorsChanged != null)
            {
                this.ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ValidateSelectedFile()
        {
            string propertyName = nameof(this.SourceFile);

            if (string.IsNullOrEmpty(this.sourceFile))
            {
                this.AddErrorToProperty(propertyName, Texts.MainViewModel_SelectFile);
            }
            else if(!this.IsValidFile())
            {
                this.AddErrorToProperty(propertyName, Texts.MainViewModel_FileNotValid);
            }
            else if (this.validationErrors.ContainsKey(propertyName))
            {
                this.validationErrors.Remove(propertyName);
                this.RaiseErrorsChanged(propertyName);
                this.NotifyOfPropertyChange(() => this.CanConvertFile);
            }
        }

        private void AddErrorToProperty(string propertyName, string errorMessage)
        {
            if (this.validationErrors.ContainsKey(propertyName))
            {
                this.validationErrors[propertyName] = errorMessage;
            }
            else
            {
                this.validationErrors.Add(propertyName, errorMessage);
            }

            this.RaiseErrorsChanged(propertyName);
            this.NotifyOfPropertyChange(() => this.CanConvertFile);
        }

        private Task RevertExeAction()
        {
            return this.attachService.RevertExe(this.SourceFile);
        }

        private Task AttachExeAction()
        {
            return this.attachService.AttachExe(this.SourceFile);
        }

        private async Task ExecuteActionWithTryCatch(Func<Task> action)
        {
            this.IsDoingAction = true;

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                await this.windowService.ShowMessageDialog(Texts.MainViewModel_Error, ex.Message);
            }
            finally
            {
                this.IsDoingAction = false;
            }
        }

        private bool IsValidFile() => this.IsExeFile() || this.IsAttachedFile();

        private bool IsExeFile() => this.SourceFile != null && this.SourceFile.EndsWith(FileConsts.ExeFileExtension, StringComparison.CurrentCultureIgnoreCase);

        private bool IsAttachedFile() => this.SourceFile != null && this.SourceFile.EndsWith(FileConsts.AttachedExtension, StringComparison.CurrentCultureIgnoreCase);
    }
}