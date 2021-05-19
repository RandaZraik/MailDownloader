using System;
using System.ComponentModel;
using System.Windows;

namespace MailDownloader
{
    /// <summary>
    /// Auto sets the view model to the data context of the matching view
    /// </summary>
    public static class ViewModelLocator
    {
        private const string AutoWireViewModelPropertyName = "AutoWireViewModel";

        public static bool GetAutoWireViewModel(DependencyObject obj) =>
            (bool)obj.GetValue(AutoWireViewModelProperty);

        public static void SetAutoWireViewModel(DependencyObject obj, bool value) =>
            obj.SetValue(AutoWireViewModelProperty, value);

        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached(
                AutoWireViewModelPropertyName,
                typeof(bool),
                typeof(ViewModelLocator),
                new PropertyMetadata(false, OnAutoWireViewModelChanged));

        private static void OnAutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            var viewTypeName = d.GetType().FullName;
            var viewModelType = Type.GetType(viewTypeName + "Model");
            viewModelType = viewModelType ?? Type.GetType(viewTypeName + "ViewModel");
            var viewModel = Activator.CreateInstance(viewModelType);
            ((FrameworkElement)d).DataContext = viewModel;
        }
    }
}
