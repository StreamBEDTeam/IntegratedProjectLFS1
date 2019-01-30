using StreamBED.Backend.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Models.ProtocolModels
{
    /// <summary>
    /// Defines the Epifaunal Substrate Model.
    /// </summary>
    [Serializable]
    public class EpifaunalSubstrateModel : ProtocolModel
    {
        /// <summary>
        /// Lists all possible keywords associated with <see cref="EpifaunalSubstrateModel"/> protocol.
        /// </summary>
        public static class Keywords
        {
            public static readonly Keyword SnagsLogs = new Keyword("Snags/Logs", "SnagsLogs");
            public static readonly Keyword UnderwaterVegetation = new Keyword("Underwater Vegetation", "UnderwaterVegetation");
            public static readonly Keyword UndercutBanks = new Keyword("Undercut Banks", "UndercutBanks");
            public static readonly Keyword Cobble = new Keyword("Small/Medium Cobble", "Cobble");
        }

        /// <summary>
        /// Returns <see cref="Keywords"/> associated with <see cref="EpifaunalSubstrateModel"/> as an array.
        /// </summary>
        public static Keyword[] GetKeywords() {
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
            if (Score >= 0 && Score <= 5)
            {
                return Category.Poor;
            }
            else if (Score >= 6 && Score <= 10) {
                return Category.Marginal;
            }
            else if (Score >= 11 && Score <= 15)
            {
                return Category.Suboptimal;
            }
            else if (Score >= 16 && Score <= 20)
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
