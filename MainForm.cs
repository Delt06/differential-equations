using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DEAssignment.Charts;
using DEAssignment.Charts.Error;
using DEAssignment.Charts.Factories;
using DEAssignment.Charts.Solution;
using DEAssignment.Methods;
using DEAssignment.Methods.Factories;
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

            var methods = _methodFactory.CreateAll().ToArray();
            var chartCollection = FunctionChartCollection.CreateViaFactory(methods, _chartFactory);

            ConfigureChartCollection(chartCollection, 0);
            Controls.AddRange(chartCollection.Select(c => c.Control).ToArray());

            var controlLeft = chartCollection.Max(c => c.Control.Right);
            var parameterControls = CreateControls(chartCollection, controlLeft);
            Controls.AddRange(parameterControls.ToArray());
        }

        private static void ConfigureChartCollection([NotNull] FunctionChartCollection chartCollection, int left)
        {
            ConfigureCharts(chartCollection.SolutionCharts, left, 0);
            var localErrorsChartTop = chartCollection.SolutionCharts.Max(c => c.Control.Bottom);
            ConfigureCharts(chartCollection.LocalErrorsCharts, left, localErrorsChartTop);
            var globalErrorsChartTop = chartCollection.LocalErrorsCharts.Max(c => c.Control.Bottom);
            ConfigureCharts(chartCollection.GlobalErrorsCharts, left,globalErrorsChartTop);
        }

        private static void ConfigureCharts([NotNull] IReadOnlyList<IFunctionChart> charts, int left, int top)
        {
            for (var i = 0; i < charts.Count; i++)
            {
                var chartControl = charts[i].Control;

                chartControl.Top = top;
                chartControl.Left = left + chartControl.Width * i;
            }
        }

        [NotNull]
        private static IEnumerable<Control> CreateControls([NotNull] FunctionChartCollection chartCollection, int left)
        {
            var (x0Label, x0Form) = new ParameterBox(left, 0, "x_0", 
                Utils.Ivp.X0, Constraint.None);
            yield return x0Label;
            yield return x0Form;
            
            var (y0Label, y0Form) = new ParameterBox(left, x0Form.Bottom, "y_0",
                Utils.Ivp.Y0, Constraint.None);
            yield return y0Label;
            yield return y0Form;

            var xMaxConstraint = new Constraint(xMax => x0Form.TryGetValue(out var x0) && xMax > x0);
            var (xMaxLabel, xMaxForm) = new ParameterBox(left, y0Form.Bottom, "x_max", 
                Utils.XMax, xMaxConstraint);
            yield return xMaxLabel;
            yield return xMaxForm;

            var nRangeConstraint = new Constraint(n => n >= 2);
            var (nLabel, nForm) = new ParameterBox(left, xMaxForm.Bottom, "n",
                Utils.N, nRangeConstraint);
            yield return nLabel;
            yield return nForm;

            var applyIvpButton = CreateApplyButton(nLabel.Left, nLabel.Bottom, 
                nLabel.Width + nForm.Width);
            yield return applyIvpButton;

            applyIvpButton.Click += (sender, args) =>
            {
                if (!IvpFormsAreValid(nForm, x0Form, y0Form, xMaxForm,
                    out var n, out var ivp, out var xMax))
                {
                    return;
                }

                var step = Utils.GetStep(ivp.X0, xMax, n);

                foreach (var chart in chartCollection.SolutionCharts)
                {
                    chart.SetUp(n, ivp, xMax);
                }

                foreach (var chart in chartCollection.LocalErrorsCharts)
                {
                    chart.Update(step, ivp, xMax);
                }
            };
            
            applyIvpButton.PerformClick();
            
            var (nMinLabel, nMinForm) = new ParameterBox(nForm.Right, 0, "n_min",
                Utils.N, nRangeConstraint);
            yield return nMinLabel;
            yield return nMinForm;
            
            var nMinConstraint = new Constraint(nMax => nMax >= 2 && 
                                                        nMinForm.TryGetValue(out var nMin) && nMax >= nMin + 1);
            var (nMaxLabel, nMaxForm) = new ParameterBox(nForm.Right, nMinLabel.Bottom, "n_max", 
                Utils.NMax, nMinConstraint);
            yield return nMaxLabel;
            yield return nMaxForm;

            var applyGlobalErrorButton = CreateApplyButton(nMaxLabel.Left, nMaxForm.Bottom, 
                nMaxLabel.Width + nMaxForm.Width);
            yield return applyGlobalErrorButton;

            applyGlobalErrorButton.Click += (sender, args) =>
            {
                if (!NRangeFormsAreValid(x0Form, y0Form, xMaxForm, nMinForm, nMaxForm, out var ivp, out var xMax,
                    out var nMin, out var nMax))
                {
                    return;
                }

                foreach (var chart in chartCollection.GlobalErrorsCharts)
                {
                    chart.Update(nMin, nMax, ivp, xMax);
                }
            };
            
            applyGlobalErrorButton.PerformClick();
        }

        private static bool IvpFormsAreValid(TextBoxWithConstraints nForm, 
            TextBoxWithConstraints x0Form,
            TextBoxWithConstraints y0Form,
            TextBoxWithConstraints xMaxForm,
            out int n,
            out Ivp ivp,
            out double xMax)
        {
            var valid = nForm.TryGetValue(out var nNotRounded) & 
                        x0Form.TryGetValue(out var x0) &
                        y0Form.TryGetValue(out var y0) &
                        xMaxForm.TryGetValue(out xMax);
            ivp = new Ivp(x0, y0);
            n = (int) nNotRounded;
            return valid;
        }

        private static bool NRangeFormsAreValid(
            TextBoxWithConstraints x0Form,
            TextBoxWithConstraints y0Form,
            TextBoxWithConstraints xMaxForm,
            TextBoxWithConstraints nMinForm,
            TextBoxWithConstraints nMaxForm,
            out Ivp ivp,
            out double xMax,
            out int nMin,
            out int nMax)
        {
            var valid = x0Form.TryGetValue(out var x0) &
                        y0Form.TryGetValue(out var y0) &
                        xMaxForm.TryGetValue(out xMax) &
                        nMinForm.TryGetValue(out var nMinNotRounded) &
                        nMaxForm.TryGetValue(out var nMaxNotRounded);
            ivp = new Ivp(x0, y0);
            nMin = (int) nMinNotRounded;
            nMax = (int) nMaxNotRounded;
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
    }
}