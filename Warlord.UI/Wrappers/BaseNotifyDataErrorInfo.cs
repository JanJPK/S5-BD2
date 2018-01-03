using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warlord.UI.ViewModel;

namespace Warlord.UI.Wrappers
{
    /// <summary>
    ///     Allows input validation.
    /// </summary>
    public class BaseNotifyDataErrorInfo : BaseVM, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, List<string>> errorsByPropertyName = new Dictionary<string, List<string>>();

        #endregion

        #region Public Events

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Public Properties

        public bool HasErrors => errorsByPropertyName.Any();

        #endregion

        #region Public Methods and Operators

        public IEnumerable GetErrors(string propertyName)
        {
            return errorsByPropertyName.ContainsKey(propertyName) ? errorsByPropertyName[propertyName] : null;
        }

        #endregion

        #region Methods

        protected void AddError(string propertyName, string error)
        {
            if (!errorsByPropertyName.ContainsKey(propertyName))
            {
                errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!errorsByPropertyName[propertyName].Contains(error))
            {
                errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (errorsByPropertyName.ContainsKey(propertyName))
            {
                errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }

        #endregion
    }
}
