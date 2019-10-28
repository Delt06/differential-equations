using System.Windows.Forms;
using JetBrains.Annotations;

namespace DEAssignment.Charts.Error
{
    public interface IHasControl
    {
        [NotNull]
        Control Control { get; }
    }
}