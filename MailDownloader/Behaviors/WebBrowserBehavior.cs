using System.Windows;
using System.Windows.Controls;

namespace MailDownloader.Behaviors
{
    /// <summary>
    /// Web browser behvior
    /// </summary>
    public class WebBrowserBehavior
    {
        private const string EmptyMailBodyText = "<html></html>";

        public static readonly DependencyProperty BodyProperty =
            DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserBehavior), new PropertyMetadata(OnChanged));

        public static string GetBody(DependencyObject dependencyObject) =>
            (string)dependencyObject.GetValue(BodyProperty);

        public static void SetBody(DependencyObject dependencyObject, string body) =>
            dependencyObject.SetValue(BodyProperty, body);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            ((WebBrowser)d).NavigateToString(!string.IsNullOrEmpty((string)e.NewValue) ? (string)e.NewValue : EmptyMailBodyText);
    }
}
