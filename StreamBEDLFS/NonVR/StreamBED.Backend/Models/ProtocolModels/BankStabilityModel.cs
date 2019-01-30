using StreamBED.Backend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Models.ProtocolModels
{
    /// <summary>
    /// Defines the Bank Stability Model.
    /// </summary>
    [Serializable]
    public class BankStabilityModel : ProtocolModel
    {
        /// <summary>
        /// Lists all possible keywords associated with <see cref="BankStabilityModel"/> protocol.
        /// </summary>
        public static class Keywords
        {
            public static readonly Keyword BankSlope = new Keyword("Bank Slope", "BankSlope");
            public static readonly Keyword BankFailure = new Keyword("Bank Failure", "BankFailure");
            public static readonly Keyword BankVegetation = new Keyword("Bank Vegetation", "BankVegetation");
            public static readonly Keyword ErodedAreas = new Keyword("Eroded Areas", "ErodedAreas");
            public static readonly Keyword ExposedTreeRoots = new Keyword("Exposed Tree Roots", "ExposedTreeRoots");
        }

        /// <summary>
        /// Returns <see cref="Keywords"/> associated with <see cref="BankStabilityModel"/> as an array.
        /// </summary>
        public static Keyword[] GetKeywords()
        {
            var original = typeof(Keywords).GetFields().Select(field => field.GetValue(typeof(Keywords)));
            var output = new List<Keyword>();

            foreach (object item in original)
            {
                if (item is Keyword)
                {
                    output.Add(item as Keyword);
                }
            }

            return output.ToArray();
        }

        /// <summary>
        /// Returns <see cref="ProtocolModel.Category"/> based on <see cref="ProtocolModel.Score"/>
        /// </summary>
        public override Category GetCategory()
        {
            if (Score >= 0 && Score <= 2)
            {
                return Category.Poor;
            }
            else if (Score >= 3 && Score <= 5)
            {
                return Category.Marginal;
            }
            else if (Score >= 6 && Score <= 8)
            {
                return Category.Suboptimal;
            }
            else if (Score >= 9 && Score <= 10)
            {
                return Category.Optimal;
            }
            else
            {
                return Category.None;
            }
        }
    }
}
