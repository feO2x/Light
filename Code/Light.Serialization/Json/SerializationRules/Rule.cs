using Light.GuardClauses;
using Light.Serialization.FrameworkExtensions;
using Light.Serialization.Json.ComplexTypeDecomposition;
using Light.Serialization.Json.WriterInstructors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Light.Serialization.Json.ObjectReferencePreservation;

namespace Light.Serialization.Json.SerializationRules
{
    public abstract class Rule : IEquatable<Rule>
    {
        public readonly Type TargetType;

        protected Rule(Type targetType)
        {
            targetType.MustNotBeNull(nameof(targetType));

            TargetType = targetType;
        }

        public bool Equals(Rule other)
        {
            if (other == null)
                return false;

            return TargetType == other.TargetType;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Rule);
        }

        public override int GetHashCode()
        {
            return TargetType.GetHashCode();
        }

        public abstract IJsonWriterInstructor CreateInstructor();
    }

    public sealed class Rule<T> : Rule, IButWhiteListRule<T>, IAndWhiteListRule<T>, IAndBlackListRule<T>
    {
        private readonly List<string> _targetMembersToSerialize = new List<string>();
        private readonly IReadableValuesTypeAnalyzer _typeAnalyzer;

        public Rule(IReadableValuesTypeAnalyzer typeAnalyzer) : base(typeof (T))
        {
            typeAnalyzer.MustNotBeNull(nameof(typeAnalyzer));

            _typeAnalyzer = typeAnalyzer;
            DeterminePublicPropertiesAndFields();
        }

        public IAndBlackListRule<T> IgnoreProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            propertyExpression.MustNotBeNull(nameof(propertyExpression));

            var propertyName = propertyExpression.ExtractPropertyName();
            _targetMembersToSerialize.Remove(propertyName);

            return this;
        }

        public IAndBlackListRule<T> IgnoreField<TField>(Expression<Func<T, TField>> fieldExpression)
        {
            fieldExpression.MustNotBeNull(nameof(fieldExpression));

            var fieldName = fieldExpression.ExtractFieldName();
            _targetMembersToSerialize.Remove(fieldName);

            return this;
        }

        IAndWhiteListRule<T> IAndWhiteListRule<T>.AndProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            AddProperty(propertyExpression);
            return this;
        }

        IAndWhiteListRule<T> IAndWhiteListRule<T>.AndField<TField>(Expression<Func<T, TField>> fieldExpression)
        {
            AddField(fieldExpression);
            return this;
        }

        IAndWhiteListRule<T> IButWhiteListRule<T>.ButProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            AddProperty(propertyExpression);
            return this;
        }

        IAndWhiteListRule<T> IButWhiteListRule<T>.ButField<TField>(Expression<Func<T, TField>> fieldExpression)
        {
            AddField(fieldExpression);
            return this;
        }

        public IButWhiteListRule<T> IgnoreAll()
        {
            _targetMembersToSerialize.Clear();
            return this;
        }

        private void DeterminePublicPropertiesAndFields()
        {
            foreach (var runtimeProperty in TargetType.GetRuntimeProperties())
            {
                var getMethod = runtimeProperty.GetMethod;
                if (getMethod == null || getMethod.IsPublic == false || getMethod.IsStatic)
                    continue;

                _targetMembersToSerialize.Add(runtimeProperty.Name);
            }

            foreach (var runtimeField in TargetType.GetRuntimeFields())
            {
                if (runtimeField.IsStatic || runtimeField.IsPublic == false)
                    continue;

                _targetMembersToSerialize.Add(runtimeField.Name);
            }
        }

        private void AddProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            propertyExpression.MustNotBeNull(nameof(propertyExpression));

            var propertyName = propertyExpression.ExtractPropertyName();
            _targetMembersToSerialize.Add(propertyName);
        }

        private void AddField<TField>(Expression<Func<T, TField>> fieldExpression)
        {
            fieldExpression.MustNotBeNull(nameof(fieldExpression));

            var fieldName = fieldExpression.ExtractFieldName();
            _targetMembersToSerialize.Add(fieldName);
        }

        public override IJsonWriterInstructor CreateInstructor()
        {
            var valueProviders = _typeAnalyzer.AnalyzeType(TargetType);

            var i = 0;
            while (i < valueProviders.Count)
            {
                if (_targetMembersToSerialize.Contains(valueProviders[i].Name) == false)
                {
                    valueProviders.RemoveAt(i);
                    continue;
                }
                i++;
            }

            return new PreserveObjectReferencesDecorator(
                new CustomRuleInstructor(TargetType, valueProviders), 
                new ObjectReferencePreserver(new Dictionary<object, uint>()));
        }
    }
}