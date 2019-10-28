using System;
using System.Globalization;
using System.Windows.Forms;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class ParameterBox
    {
        [NotNull] public readonly Label Label;
        [NotNull] public readonly TextBoxWithConstraints TextBox;

        public ParameterBox(int left, int top, [NotNull] string label, double initialValue,
            [NotNull] Constraint constraint)
        {
            if (label is null) throw new ArgumentNullException(nameof(label));
            if (constraint is null) throw new ArgumentNullException(nameof(constraint));

            Label = new Label
            {
                Text = label, 
                Left = left,
                Top = top,
            };
            
            TextBox = new TextBoxWithConstraints(constraint)
            {
                Text = initialValue.ToString(CultureInfo.InvariantCulture), 
                Left = Label.Right,
                Top = top
            };
        }

        public void Deconstruct([NotNull] out Label label, [NotNull] out TextBoxWithConstraints textBox)
        {
            label = Label;
            textBox = TextBox;
        }
    }
}