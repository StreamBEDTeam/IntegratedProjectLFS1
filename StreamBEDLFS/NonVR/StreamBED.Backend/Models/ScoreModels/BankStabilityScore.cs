using StreamBED.Backend.Models.ProtocolModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Models.ScoreModels
{
    /// <summary>
    /// Defines score associated with <see cref="BankStabilityModel"/>
    /// </summary>
    [Serializable]
    internal class BankStabilityScore : ScoreModel
    {
        /// <summary>
        /// Changes <see cref="BankStabilityScore"/>.
        /// </summary>
        /// <param name="score"></param>
        public override void ChangeScore(int score)
        {
            if (score <= 10 || score >= 0)
            {
                Score = score;
            }
        }
    }
}
