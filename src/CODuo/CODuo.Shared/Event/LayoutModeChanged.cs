namespace CODuo.Event
{
    public class LayoutModeChanged
    {
        public LayoutModeChanged(Platform.Layout.Mode mode)
        {
            Mode = mode;
        }

        public Platform.Layout.Mode Mode { get; }
    }
}
