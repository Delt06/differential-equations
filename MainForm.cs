using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DEAssignment.Methods;
using DEAssignment.Methods.Factories;
using DEAssignment.SolutionCharts;
using DEAssignment.SolutionCharts.Factories;
using JetBrains.Annotations;

namespace DEAssignment
{
    public class MainForm : Form
    {
        public MainForm([NotNull] IMethodChartFactory chartFactory, [NotNull] IMethodFactory methodFactory)
        {
            _chartFactory = chartFactory ?? throw new ArgumentNullException(nameof(chartFactory));
            _methodFactory = methodFactory ?? throw new ArgumentNullException(nameof(methodFactory));

            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public const int Columns = 2;
        public const int Rows = 2;

        [NotNull]
        private readonly IMethodChartFactory _chartFactory;
        [NotNull]
        private readonly IMethodFactory _methodFactory;

        protected override void OnLoad(EventArgs e)
        {
            AutoSize = true;

            var methods = _methodFactory.CreateAll();
            
            var charts = methods
                .Select(_chartFactory.CreateChartFor)
                .ToArray();

            ConfigureCharts(charts);
            
            foreach (var chart in charts)
            {
                Controls.Add(chart.Control);
            }

            foreach (var control in CreateControls(charts, charts.Max(c => c.Control.Left + c.Control.Width)))
            {
                Controls.Add(control);
            }
        }

        private static void ConfigureCharts([NotNull] IReadOnlyList<ISolvingMethodChart> charts)
        {
            for (var i = 0; i < charts.Count; i++)
            {
                var chartControl = charts[i].Control;

                chartControl.Top = chartControl.Height * (i / Rows);
                chartControl.Left = chartControl.Width * (i % Columns);
            }
        }

        [NotNull]
        private static IEnumerable<Control> CreateControls(ISolvingMethodChart[] charts, int left)
        {
            var (stepLabel, stepForm) = new ParameterBox(left, 0, "step",
                Utils.Step, RangeConstraint.ZeroOne);
            yield return stepLabel;
            yield return stepForm;
            
            var (x0Label, x0Form) = new ParameterBox(left, stepForm.Bottom, "x_0", 
                Utils.Ivp.X0, RangeConstraint.None);
            yield return x0Label;
            yield return x0Form;
            
            var (y0Label, y0Form) = new ParameterBox(left, x0Form.Bottom, "y_0",
                Utils.Ivp.Y0, RangeConstraint.None);
            yield return y0Label;
            yield return y0Form;
            
            var xMaxConstraint = new RangeConstraint(
                () => x0Form.TryGetValue(out var y0) ? y0 : double.NegativeInfinity, 
                () => double.PositiveInfinity);
            var (xMaxLabel, xMaxForm) = new ParameterBox(left, y0Form.Bottom, "x_max", 
                Utils.XMax, xMaxConstraint);
            yield return xMaxLabel;
            yield return xMaxForm;

            var button = CreateApplyButton(xMaxLabel.Left, xMaxLabel.Bottom, 
                xMaxLabel.Width + xMaxForm.Width);
            yield return button;

            var table = CreateTableForErrors(x0Form.Right);
            yield return table;

            button.Click += (sender, args) =>
            {
                if (!AreValid(stepForm, x0Form, y0Form, xMaxForm,
                    out var step, out var ivp, out var xMax))
                {
                    return;
                }

                foreach (var chart in charts)
                {
                    chart.SetUp(step, ivp, xMax);
                }
                
                var approximatedMethods = charts
                    .Select(c => c.Method)
                    .Where(m => m is ApproximatedMethod)
                    .ToArray();
                table.SetUpForErrors(approximatedMethods, step, ivp, xMax);
            };
            
            button.PerformClick();
        }

        private static bool AreValid(TextBoxWithConstraints stepForm, 
            TextBoxWithConstraints x0Form,
            TextBoxWithConstraints y0Form,
            TextBoxWithConstraints xMaxForm,
            out double step,
            out Ivp ivp,
            out double xMax)
        {
            var valid = stepForm.TryGetValue(out step) & 
                        x0Form.TryGetValue(out var x0) &
                        y0Form.TryGetValue(out var y0) &
                        xMaxForm.TryGetValue(out xMax);
            ivp = new Ivp(x0, y0);
            return valid;
        }

        [NotNull]
        private static Button CreateApplyButton(int left, int top, int width)
        {
            return new Button()
            {
                Text = "Apply",
                Top = top,
                Left = left,
                Width = width
            };
        }

        private static DataGridView CreateTableForErrors(int left)
        {
            return new DataGridView()
            {
                Left = left,
                Width = 700,
                Height = 1000
            };
        }
    }
}