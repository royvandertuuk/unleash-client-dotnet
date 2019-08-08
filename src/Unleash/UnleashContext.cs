using System;
using System.Linq;

namespace Unleash
{
    using System.Collections.Generic;

    /// <summary>
    /// A context which the feature request should be validated againt. Usually scoped to a web request through an implementation of IUnleashContextProvider.
    /// </summary>
    public class UnleashContext
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string RemoteAddress { get; set; }
        public Dictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Appends the given properties to the <see cref="Properties"/> property.
        /// Throws an <see cref="ArgumentException"/> if the key of a property is already in the <see cref="Properties"/> property.
        /// </summary>
        /// <param name="properties"></param>
        internal void AppendProperties(Dictionary<string, string> properties)
        {
            foreach (var property in properties)
            {
                if (Properties.ContainsKey(property.Key))
                {
                    throw new ArgumentException($"A property with the key: {property.Key} already exists in {nameof(Properties)}", nameof(properties));
                }

                Properties.Add(property.Key, property.Value);
            }
        }

        internal UnleashContext Clone()
        {
            return new UnleashContext
            {
                UserId = UserId,
                SessionId = SessionId,
                RemoteAddress = RemoteAddress,
                Properties = Properties.ToDictionary(p => p.Key, p => p.Value)
            };
        }

        #region Builder pattern: used in tests

        internal static Builder New()
        {
            return new Builder();
        }

        internal class Builder
        {
            private string userId;
            private string sessionId;
            private string remoteAddress;
            private readonly Dictionary<string, string> properties = new Dictionary<string, string>();

            public Builder UserId(string userId)
            {
                this.userId = userId;
                return this;
            }

            public Builder SessionId(string sessionId)
            {
                this.sessionId = sessionId;
                return this;
            }

            public Builder RemoteAddress(string remoteAddress)
            {
                this.remoteAddress = remoteAddress;
                return this;
            }

            public Builder AddProperty(string name, string value)
            {
                properties.Add(name, value);
                return this;
            }

            public UnleashContext Build()
            {
                return new UnleashContext()
                {
                    UserId = userId,
                    SessionId = sessionId,
                    RemoteAddress = remoteAddress,
                    Properties = properties,
                };
            }
        }

       
    }

    #endregion
}