using System.Collections.Generic;

namespace System.Linq
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Runs an action for each item in the collection
        /// </summary>
        /// <typeparam name="TItem">Type of the item</typeparam>
        /// <param name="enumeration">Enumeration to run through</param>
        /// <param name="action">Action to execute the item on</param>
        public static void ForEach<TItem>(this IEnumerable<TItem> enumeration, Action<TItem> action)
        {
            foreach (var item in enumeration)
            {
                action(item);
            }
        }
    }
}
