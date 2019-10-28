using System;
using System.Windows.Forms;
using DEAssignment.Methods;
using DEAssignment.Methods.Errors;
using DEAssignment.Methods.Visitors;
using JetBrains.Annotations;

namespace DEAssignment
{
    public static class DataGridViewExtensions
    {
        public static void SetUpForErrors([NotNull] this DataGridView table, [NotNull] ISolvingMethod[] methods, double step, Ivp ivp, double xMax)
        {
            if (table is null) throw new ArgumentNullException(nameof(table));
            if (methods is null) throw new ArgumentNullException(nameof(methods));
            
            SetTableSize(table, methods, step, ivp, xMax);
            ShowHeading(table, methods);
            ShowErrorIndices(table);
            ShowErrors(table, methods, step, ivp, xMax);
        }
        private static void SetTableSize(DataGridView table, ISolvingMethod[] methods, double step, Ivp ivp, double xMax)
        {
            table.ColumnCount = 1 + methods.Length * 2;
            table.RowCount = 1 + Utils.GetPointsCount(ivp.X0, xMax, step);
        }

        private static void ShowHeading(DataGridView table, ISolvingMethod[] methods)
        {
            var visitor = new GetNameVisitor();
            
            for (var i = 0; i < methods.Length; i++)
            {
                methods[i].Accept(visitor);
                var name = visitor.Result;

                var globalCell = table[1 + i * 2, 0];
                globalCell.ValueType = typeof(string);
                globalCell.Value = name + " (Global)";

                var localCell = table[1 + i * 2 + 1, 0];
                localCell.ValueType = typeof(string);
                localCell.Value = name + " (Local)";
            }
        }

        private static void ShowErrorIndices(DataGridView table)
        {
            for (var i = 1; i < table.RowCount; i++)
            {
                var cell = table[0, i];
                cell.ValueType = typeof(int);
                cell.Value = i - 1;
            }
        }

        private static void ShowErrors(DataGridView table, ISolvingMethod[] methods, double step, Ivp ivp, double xMax)
        {
            for (var methodIndex = 0; methodIndex < methods.Length; methodIndex++)
            {
                var localErrors = methods[methodIndex].GetLocalErrors(step, ivp, xMax, out var globalErrors);

                for (var errorIndex = 0; errorIndex < globalErrors.Length; errorIndex++)
                {
                    var globalCell = table[1 + 2 * methodIndex, errorIndex + 1];
                    globalCell.ValueType = typeof(double);
                    globalCell.Value = globalErrors[errorIndex];

                    var localCell = table[1 + 2 * methodIndex + 1, errorIndex + 1];
                    localCell.ValueType = typeof(double);
                    localCell.Value = localErrors[errorIndex];
                }
            }
        }
    }
}