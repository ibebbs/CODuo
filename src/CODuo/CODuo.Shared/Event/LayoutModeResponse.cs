namespace CODuo.Event
{

    public class LayoutModeResponse
    {
        public LayoutModeResponse(Platform.Layout.Mode mode)
        {
            Mode = mode;
        }

        public Platform.Layout.Mode Mode { get; }
    }
}
