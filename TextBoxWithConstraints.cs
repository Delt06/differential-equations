using System;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class TextBoxWithConstraints : TextBox
    {
        [NotNull] private readonly Constraint _constraint;

        public TextBoxWithConstraints([NotNull] Constraint constraint)
        {
            _constraint = constraint ?? throw new ArgumentNullException(nameof(constraint));
        }


        public bool TryGetValue(out double value)
        {
            var valid = double.TryParse(Text, out value) && _constraint.IsSatisfied(value);
            
            if (!valid)
            {
                Text = ErrorString;
            }

            return valid;
        }

        private const string ErrorString = "error";
    }
}