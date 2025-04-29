using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StringFormatting
{
    public interface IStringFormatter
    {
        string Format(string template, object target);
    }

    public class StringFormatter : IStringFormatter
    {
        public static readonly StringFormatter Shared = new StringFormatter();

        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<object, string>>> _cache
            = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<object, string>>>();

        private StringFormatter() { }

        public string Format(string template, object target)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            if (target == null) throw new ArgumentNullException(nameof(target));

            var sb = new StringBuilder();
            int i = 0, len = template.Length;

            while (i < len)
            {
                char c = template[i];
                if (c == '{')
                {
                    if (i + 1 < len && template[i + 1] == '{')
                    {
                        sb.Append('{');
                        i += 2;
                    }
                    else
                    {
                        int close = template.IndexOf('}', i + 1);
                        if (close < 0)
                            throw new FormatException("Не найдена закрывающая '}' для placeholder-а.");

                        string token = template.Substring(i + 1, close - i - 1);
                        if (string.IsNullOrWhiteSpace(token))
                            throw new FormatException("Пустой placeholder недопустим.");

                        string replacement = GetAccessor(target.GetType(), token)(target);
                        sb.Append(replacement);
                        i = close + 1;
                    }
                }
                else if (c == '}')
                {
                    if (i + 1 < len && template[i + 1] == '}')
                    {
                        sb.Append('}');
                        i += 2;
                    }
                    else
                    {
                        throw new FormatException("Неожиданная '}' без пары.");
                    }
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }


        private Func<object, string> GetAccessor(Type targetType, string token)
        {
            var dict = _cache.GetOrAdd(targetType, _ => new ConcurrentDictionary<string, Func<object, string>>());
            return dict.GetOrAdd(token, t => CreateAccessor(targetType, t));
        }

        private Func<object, string> CreateAccessor(Type targetType, string token)
        {
            int bracketPos = token.IndexOf('[');
            MemberInfo member;
            Expression memberAccess;
            var param = Expression.Parameter(typeof(object), "target");
            var instance = Expression.Convert(param, targetType);

            if (bracketPos > 0)
            {
                if (!token.EndsWith("]"))
                    throw new FormatException($"Неправильный синтаксис индекса в '{token}'.");

                string propName = token.Substring(0, bracketPos);
                string idxString = token.Substring(bracketPos + 1, token.Length - bracketPos - 2);

                if (!int.TryParse(idxString, out int index))
                    throw new FormatException($"Индекс не является числом: '{idxString}'.");

                member = (MemberInfo)targetType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance)
                         ?? targetType.GetField(propName, BindingFlags.Public | BindingFlags.Instance)
                         ?? throw new MissingMemberException($"'{propName}' не найден в {targetType.Name}.");

                memberAccess = member is PropertyInfo pi
                    ? Expression.Property(instance, pi)
                    : Expression.Field(instance, (FieldInfo)member);

                Expression indexed;
                if (memberAccess.Type.IsArray)
                {
                    indexed = Expression.ArrayIndex(memberAccess, Expression.Constant(index));
                }
                else
                {
                    var idxProp = memberAccess.Type.GetProperty("Item", new[] { typeof(int) })
                                  ?? throw new InvalidOperationException($"Тип {memberAccess.Type.Name} не поддерживает индексатор [int].");
                    indexed = Expression.Property(memberAccess, idxProp, Expression.Constant(index));
                }

                var toObj = Expression.Convert(indexed, typeof(object));
                var toStringCall = Expression.Call(toObj, nameof(object.ToString), Type.EmptyTypes);
                return Expression.Lambda<Func<object, string>>(toStringCall, param).Compile();
            }
            else
            {
                member = (MemberInfo)targetType.GetProperty(token, BindingFlags.Public | BindingFlags.Instance)
                         ?? targetType.GetField(token, BindingFlags.Public | BindingFlags.Instance)
                         ?? throw new MissingMemberException($"'{token}' не найден в {targetType.Name}.");

                memberAccess = member is PropertyInfo pi
                    ? Expression.Property(instance, pi)
                    : Expression.Field(instance, (FieldInfo)member);

                var toObj = Expression.Convert(memberAccess, typeof(object));
                var toStringCall = Expression.Call(toObj, nameof(object.ToString), Type.EmptyTypes);
                return Expression.Lambda<Func<object, string>>(toStringCall, param).Compile();
            }
        }
    }
}