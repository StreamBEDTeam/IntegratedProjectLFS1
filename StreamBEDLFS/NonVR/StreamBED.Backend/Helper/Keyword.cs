using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Helper
{
    /// <summary>
    /// Defines a keyword.
    /// </summary>
    [Serializable]
    public class Keyword : Attribute
    {
        /// <summary>
        /// Represents <see cref="Keyword"/> content.
        /// </summary>
        public string Content { get; }

        public string FriendlyName { get; }

        // Internal constructor
        internal Keyword(string Content, string FriendlyName)
        {
            this.Content = Content;
            this.FriendlyName = FriendlyName;
        }
    }
}
