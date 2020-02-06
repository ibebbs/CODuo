using System.Numerics;

namespace CODuo.Platform.Layout
{
    public class Instance : ILayout
    {
        public Instance(Vector4 pane1Location, double dipScaling)
        {
            Mode = Mode.Single;

            Pane1Visible = true;
            Pane1Location = pane1Location;

            Pane2Visible = false;
            Pane2Location = Vector4.Zero;

            DipScaling = dipScaling;
        }

        public Instance(Mode mode, Vector4 pane1Location, Vector4 pane2Location, double dipScaling)
        {
            Mode = mode;

            Pane1Visible = true;
            Pane1Location = pane1Location;

            Pane2Visible = true;
            Pane2Location = pane2Location;
        }

        public Mode Mode { get; }

        public bool Pane1Visible { get; }

        public Vector4 Pane1Location { get; }

        public bool Pane2Visible { get; }

        public Vector4 Pane2Location { get; }

        public double DipScaling { get; }
    }
}
