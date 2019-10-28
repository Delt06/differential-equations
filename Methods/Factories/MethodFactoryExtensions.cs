using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DEAssignment.Methods.Factories
{
    public static class MethodFactoryExtensions
    {
        [NotNull]
        public static IEnumerable<ISolvingMethod> CreateAll([NotNull] this IMethodFactory factory)
        {
            if (factory is null) throw new ArgumentNullException(nameof(factory));

            yield return factory.CreateExactMethod();
            yield return factory.CreateEulerMethod();
            yield return factory.CreateImprovedEulerMethod();
            yield return factory.CreateClassicRungeKuttaMethod();
        }
    }
}