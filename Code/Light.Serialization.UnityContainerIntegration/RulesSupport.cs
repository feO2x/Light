using System;
using Light.GuardClauses;
using Light.Serialization.Json.SerializationRules;
using Microsoft.Practices.Unity;

namespace Light.Serialization.UnityContainerIntegration
{
    public static class RulesSupport
    {
        public static IUnityContainer WithSerializationRuleFor<T>(this IUnityContainer container, Action<Rule<T>> configureRule)
        {
            container.MustNotBeNull(nameof(container));
            configureRule.MustNotBeNull(nameof(configureRule));

            var newRule = container.Resolve<Rule<T>>();
            configureRule(newRule);
            var customInstructor = newRule.CreateInstructor();
            container.RegisterInstance(typeof (T).FullName, customInstructor);
            return container;
        }

        public static IUnityContainer WithSerializationRuleFor<T>(this IUnityContainer container, Rule<T> rule)
        {
            container.MustNotBeNull(nameof(container));
            rule.MustNotBeNull(nameof(rule));

            var customInstructor = rule.CreateInstructor();
            container.RegisterInstance(typeof (T).FullName, customInstructor);
            return container;
        }
    }
}