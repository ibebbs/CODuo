namespace CODuo.Event.Layout
{
    public class Changed
    {
        public Changed(Root.Layout layout)
        {
            Layout = layout;
        }

        public Root.Layout Layout { get; }
    }
}
