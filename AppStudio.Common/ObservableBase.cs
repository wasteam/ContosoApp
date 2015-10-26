// ***********************************************************************
// <copyright file="ObservableBase.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// This class implements the INotifyPropertyChanged interface.
    /// </summary>
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets the property value and optionally raises the PropertyChanged event.
        /// </summary>
        /// <typeparam name="T">The type of the data being stored.</typeparam>
        /// <param name="storage">The backing field where the data will be saved.</param>
        /// <param name="value">The value to be saved.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns><c>true</c> if the value was changed, <c>false</c> otherwise.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                try
                {
                    eventHandler(this, new PropertyChangedEventArgs(propertyName));
                }
                catch { }
            }
        }
    }
}
