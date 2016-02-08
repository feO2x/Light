using System.Collections.Generic;
using System.Text;

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
                var itemToAppend = items[i];
                stringBuilder.Append(itemToAppend != null ? itemToAppend.ToString() : "null");
                if (i < items.Count - 1)
                    stringBuilder.Append(", ");
            }

            return stringBuilder;
        }
    }
}
