using StreamBED.Backend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Models
{
    /// <summary>
    /// Defines an abstract class to represent a protocol.
    /// </summary>
    [Serializable]
    public abstract class ProtocolModel
    {
        /// <summary>
        /// Lists all possible categories associated with a protocol.
        /// </summary>
        public enum Category
        {
            Optimal,
            Suboptimal,
            Marginal,
            Poor,
            None
        }

        /// <summary>
        /// Score associated with the protocol.
        /// </summary>
        public double Score { get; set; }

        public bool IsCompleted = false;

        public bool IsFinalCompleted = false;

        /// <summary>
        /// Returns <see cref="ProtocolModel.Category"/> based on <see cref="ProtocolModel.Score"/>
        /// </summary>
        public abstract Category GetCategory();
    }
}
