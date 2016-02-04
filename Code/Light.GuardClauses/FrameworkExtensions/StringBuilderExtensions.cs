using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.GuardClauses.FrameworkExtensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendItems<T>(this StringBuilder stringBuilder, IReadOnlyList<T> items, string itemSeperators = ", ")
        {
            stringBuilder.MustNotBeNull(nameof(stringBuilder));
            items.MustNotBeNullOrEmpty(nameof(items));

            for (var i = 0; i < items.Count; i++)
            {
                stringBuilder.Append(items[i]);
                if (i < items.Count - 1)
                    stringBuilder.Append(", ");
            }

            return stringBuilder;
        }
    }
}
