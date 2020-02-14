using System;
using System.ComponentModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CODuo.Controls
{
    public enum ContactType
    {
        Phone,
        Web,
        EMail,
        Twitter
    }

    public sealed partial class ContactButton : UserControl
    {
        public static readonly DependencyProperty ContactTypeProperty = DependencyProperty.Register("ContactType", typeof(ContactType), typeof(ContactButton), new PropertyMetadata(ContactType.Phone, ContactTypePropertyChanged));
        public static readonly DependencyProperty ContactUriProperty = DependencyProperty.Register("ContactUri", typeof(string), typeof(ContactButton), new PropertyMetadata(string.Empty, ContactUriPropertyChanged));
        public static readonly DependencyProperty HyperlinkEnabledProperty = DependencyProperty.Register("HyperlinkEnabled", typeof(bool), typeof(ContactButton), new PropertyMetadata(false));
        public static readonly DependencyProperty HyperlinkOpacityProperty = DependencyProperty.Register("HyperlinkOpacity", typeof(double), typeof(ContactButton), new PropertyMetadata(0.5));

        public static readonly DependencyProperty PhoneVisibilityProperty = DependencyProperty.Register(nameof(PhoneVisibility), typeof(Visibility), typeof(ContactButton), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty WebVisibilityProperty = DependencyProperty.Register(nameof(WebVisibility), typeof(Visibility), typeof(ContactButton), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty EMailVisibilityProperty = DependencyProperty.Register(nameof(EMailVisibility), typeof(Visibility), typeof(ContactButton), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty TwitterVisibilityProperty = DependencyProperty.Register(nameof(TwitterVisibility), typeof(Visibility), typeof(ContactButton), new PropertyMetadata(Visibility.Visible));

        private static void ContactUriPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContactButton button)
            {
                if (e.NewValue is string contactUri && !string.IsNullOrEmpty(contactUri))
                {
                    button.SetValue(HyperlinkEnabledProperty, true);
                    button.SetValue(HyperlinkOpacityProperty, 1.0);
                }
                else
                {
                    button.SetValue(HyperlinkEnabledProperty, false);
                    button.SetValue(HyperlinkOpacityProperty, 0.5);
                }
            }
        }

        private static void SetVisibility(ContactButton button)
        {
            button.SetValue(PhoneVisibilityProperty, button.ContactType == ContactType.Phone ? Visibility.Visible : Visibility.Collapsed);
            button.SetValue(WebVisibilityProperty, button.ContactType == ContactType.Web ? Visibility.Visible : Visibility.Collapsed);
            button.SetValue(EMailVisibilityProperty, button.ContactType == ContactType.EMail ? Visibility.Visible : Visibility.Collapsed);
            button.SetValue(TwitterVisibilityProperty, button.ContactType == ContactType.Twitter ? Visibility.Visible : Visibility.Collapsed);
        }

        private static void ContactTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContactButton button)
            {
                SetVisibility(button);
            }
        }

        public ContactButton()
        {
            this.InitializeComponent();
        }

        private async void HyperlinkTapped(object sender, TappedRoutedEventArgs args)
        {
            switch (ContactType)
            {
                case ContactType.Phone:
                    Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(ContactUri, string.Empty);
                    break;
                case ContactType.EMail:
                    await Launcher.LaunchUriAsync(new Uri($"mailto://{ContactUri}"));
                    break;
                case ContactType.Web:
                    await Launcher.LaunchUriAsync(new Uri($"https://{ContactUri}"));
                    break;
                case ContactType.Twitter:
                    var uri = new Uri($"twitter://userName?user_id={ContactUri}");
                    if (await Launcher.QueryUriSupportAsync(uri, LaunchQuerySupportType.Uri) == LaunchQuerySupportStatus.Available)
                    {
                        await Launcher.LaunchUriAsync(uri);
                    }
                    else
                    {
                        uri = new Uri($"https://twitter.com/{ContactUri}");
                        await Launcher.LaunchUriAsync(uri);
                    }
                    break;
            }
        }

        public bool HyperlinkEnabled
        {
            get { return (bool)GetValue(HyperlinkEnabledProperty); }
        }

        public double HyperlinkOpacity
        {
            get { return (double)GetValue(HyperlinkOpacityProperty); }
        }

        public Visibility PhoneVisibility
        {
            get { return (Visibility)GetValue(PhoneVisibilityProperty); }
        }

        public Visibility WebVisibility
        {
            get { return (Visibility)GetValue(WebVisibilityProperty); }
        }

        public Visibility EMailVisibility
        {
            get { return (Visibility)GetValue(EMailVisibilityProperty); }
        }

        public Visibility TwitterVisibility
        {
            get { return (Visibility)GetValue(TwitterVisibilityProperty); }
        }

        public ContactType ContactType
        {
            get { return (ContactType)GetValue(ContactTypeProperty); }
            set { SetValue(ContactTypeProperty, value); }
        }

        public string ContactUri
        {
            get { return (string)GetValue(ContactUriProperty); }
            set { SetValue(ContactUriProperty, value); }
        }
    }
}
