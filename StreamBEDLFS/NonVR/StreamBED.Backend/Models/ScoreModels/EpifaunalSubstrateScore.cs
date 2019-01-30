using StreamBED.Backend.Models.ProtocolModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Models.ScoreModels
{
    /// <summary>
    /// Defines score associated with <see cref="EpifaunalSubstrateModel"/>
    /// </summary>
    [Serializable]
    internal class EpifaunalSubstrateScore : ScoreModel
    {
        /// <summary>
        /// Changes <see cref="EpifaunalSubstrateScore"/>.
        /// </summary>
        /// <param name="score"></param>
        public override void ChangeScore(int score)
        {
            if (score <= 20 || score >= 0)
            {
                Score = score;
            }
        }
    }
}
