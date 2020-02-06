namespace CODuo.Event
{
    public class LayoutChanged
    {
        public LayoutChanged(Root.Layout layout)
        {
            Layout = layout;
        }

        public Root.Layout Layout { get; }
    }
}
