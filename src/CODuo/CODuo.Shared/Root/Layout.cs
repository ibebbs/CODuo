using System.Numerics;

namespace CODuo.Root
{
    public class Layout
    {
        public Layout(Vector4 pane1Location, object pane1Content, double dipScaling)
        {
            Pane1Visible = true;
            Pane1Location = pane1Location;
            Pane1Content = pane1Content;

            Pane2Visible = false;
            Pane2Location = Vector4.Zero;
            Pane2Content = null;

            DipScaling = dipScaling;
        }

        public Layout(Vector4 pane1Location, object pane1Content, Vector4 pane2Location, object pane2Content, double dipScaling)
        {
            Pane1Visible = true;
            Pane1Location = pane1Location;
            Pane1Content = pane1Content;

            Pane2Visible = true;
            Pane2Location = pane2Location;
            Pane2Content = pane2Content;

            DipScaling = dipScaling;
        }

        public bool Pane1Visible { get; }

        public Vector4 Pane1Location { get; }

        public object Pane1Content { get; }

        public bool Pane2Visible { get; }

        public Vector4 Pane2Location { get; }

        public object Pane2Content { get; }

        public double DipScaling { get; }
    }
}
