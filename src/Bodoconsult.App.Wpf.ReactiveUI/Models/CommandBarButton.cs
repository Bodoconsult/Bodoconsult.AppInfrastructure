using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Bodoconsult.App.Wpf.ReactiveUI.Models
{



    /// <summary>
    /// Contains all information needed to create a command bar button
    /// </summary>
    public class CommandBarButton: PropertyChangedBase
    {
        /// <summary>
        /// Visible name of the button
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path to a image for the button
        /// </summary>
        public Uri ImageUri { get; set; }


        /// <summary>
        /// Command to execute when button clicked
        /// </summary>
        public ICommand Command { get; set; }


    }

    /// <summary>
    /// Base class for classes implementing INotifyPropertyChanged
    /// </summary>
    public abstract class PropertyChangedBase: INotifyPropertyChanged
    {
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property changed event
        /// </summary>
        /// <param name="propertyName">Current property name</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Set a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
