using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class TextBoxWithConstraints : TextBox
    {
        [NotNull] private readonly RangeConstraint _constraint;

        public TextBoxWithConstraints([NotNull] RangeConstraint constraint)
        {
            _constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }


        public bool TryGetValue(out double value)
        {
            var valid = double.TryParse(Text, out value) &&
                        _constraint.MinValue <= value && value <= _constraint.MaxValue;
            
            if (!valid)
            {
                Text = ErrorString;
            }

            return valid;
        }

        private const string ErrorString = "error";
    }
}