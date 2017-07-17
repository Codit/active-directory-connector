using System;

namespace Codit.ApiApps.Common
{
    public class Maybe<TValue>
    {
        private readonly TValue _value;

        public Maybe()
        {
            IsPresent = false;
        }

        public Maybe(TValue value)
        {
            _value = value;

            // If we have a value type, it's always present.
            if (typeof(TValue).IsValueType)
            {
                IsPresent = true;
            }
            else // If it's a reference type, it's present if the value is not null
            {
                IsPresent = _value != null;
            }
        }

        /// <summary>
        ///     Indication wheter or not there is a value present
        /// </summary>
        public bool IsPresent { get; }

        /// <summary>
        ///     The found value, if applicable
        /// </summary>
        public TValue Value
        {
            get
            {
                if (_value == null)
                {
                    throw new InvalidOperationException($"No value is present. Expected a value of type {typeof(TValue).Name}");
                }

                return _value;
            }
        }
    }
}
