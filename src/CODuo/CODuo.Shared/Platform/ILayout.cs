using System.Numerics;

namespace CODuo.Platform
{
    public interface ILayout
    {
        Layout.Mode Mode { get; }

        bool Pane1Visible { get; }

        Vector4 Pane1Location { get; }

        bool Pane2Visible { get; }

        Vector4 Pane2Location { get; }

        double DipScaling { get; }
    }
}
