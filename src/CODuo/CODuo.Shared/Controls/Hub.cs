using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace CODuo.Controls
{
    [TemplatePart(Name = "HubSections", Type = typeof(StackPanel))]
    [ContentProperty(Name = "Sections")]
    public partial class Hub : Control
    {
        private ObservableCollection<HubSection> _hubSections = new ObservableCollection<HubSection>();

        public Hub()
        {
            this.DefaultStyleKey = typeof(Hub);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var panel = (StackPanel)GetTemplateChild("HubSections");

            panel.Children.Clear();

            foreach (var section in _hubSections)
            {
                panel.Children.Add(section);
            }
        }

        public IList<HubSection> Sections => _hubSections;
    }

    [ContentProperty(Name = "ContentTemplate")]
    public partial class HubSection : Control
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(HubSection), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(HubSection), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(HubSection), new PropertyMetadata(null));

        public HubSection()
        {
            this.DefaultStyleKey = typeof(HubSection);
        }

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
    }
}
