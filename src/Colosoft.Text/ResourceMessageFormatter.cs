using System;

namespace Colosoft
{
    public sealed class ResourceMessageFormatter : IMessageFormattable, ICloneable
    {
        public string BaseName { get; set; }

        public string Name { get; set; }

        public Type ResourceType { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
        public object[] Parameters { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

        public ResourceMessageFormatter()
        {
        }

        public ResourceMessageFormatter(string baseName, string name, Type resourceType, params object[] parameters)
        {
            if (string.IsNullOrEmpty(baseName))
            {
                throw new ArgumentNullException(nameof(baseName));
            }
            else if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (resourceType == null)
            {
                throw new ArgumentNullException(nameof(resourceType));
            }

            this.BaseName = baseName;
            this.Name = name;
            this.ResourceType = resourceType;
            this.Parameters = parameters;
        }

        public static ResourceMessageFormatter Create(System.Linq.Expressions.Expression<Func<string>> propertySelector, params object[] parameters)
        {
            if (propertySelector == null)
            {
                throw new ArgumentNullException(nameof(propertySelector));
            }

            var propertyExpression = (System.Linq.Expressions.MemberExpression)propertySelector.Body;
            var resourceType = propertyExpression.Member.DeclaringType;
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            var resourceManagerProperty = resourceType.GetProperty(
                "ResourceManager",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

            var resourceManager = (System.Resources.ResourceManager)resourceManagerProperty.GetValue(null, null);

            return new ResourceMessageFormatter(resourceManager.BaseName, propertySelector.GetMember().Name, resourceType, parameters);
        }

        string IMessageFormattable.Format(System.Globalization.CultureInfo culture)
        {
            return this.Format(culture, this.Parameters);
        }

        public IMessageFormattable Join(string separator, IMessageFormattable message)
        {
            return new Text.JoinMessageFormattable(this, separator, message);
        }

        public string Format(System.Globalization.CultureInfo culture, params object[] parameters)
        {
#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            var resourceManagerProperty = this.ResourceType.GetProperty(
                "ResourceManager",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields
            var resourceManager = (System.Resources.ResourceManager)resourceManagerProperty.GetValue(null, null);

            var text = resourceManager.GetString(this.Name, culture);

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            object[] values = null;

            if (parameters != null)
            {
                values = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var obj = parameters[i];
                    if (obj is IMessageFormattable formattable)
                    {
                        values[i] = formattable.Format(culture);
                    }
                    else
                    {
                        values[i] = obj;
                    }
                }
            }

            if (values != null && values.Length > 0)
            {
                return string.Format(culture, text, values);
            }

            return text;
        }

        public string Format()
        {
            return this.Format(System.Threading.Thread.CurrentThread.CurrentCulture, this.Parameters);
        }

        public bool Matches(System.Globalization.CultureInfo culture)
        {
            return true;
        }

        public bool Equals(IMessageFormattable other)
        {
            var other2 = other as ResourceMessageFormatter;

            if (other2 != null &&
                other2.BaseName == this.BaseName &&
                other2.Name == this.Name &&
                other2.ResourceType == this.ResourceType)
            {
                if (other2.Parameters == null && this.Parameters == null)
                {
                    return true;
                }

                if (other2.Parameters.Length != this.Parameters.Length)
                {
                    return false;
                }

                for (var i = 0; i < this.Parameters.Length; i++)
                {
                    if (this.Parameters[i] != other2.Parameters[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return this.Format();
        }

        public object Clone()
        {
            return new ResourceMessageFormatter(this.BaseName, this.Name, this.ResourceType, this.Parameters);
        }
    }
}
