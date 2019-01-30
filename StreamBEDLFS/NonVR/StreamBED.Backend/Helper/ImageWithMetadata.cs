using StreamBED.Backend.Models;
using StreamBED.Backend.Models.ScoreModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Helper
{
    /// <summary>
    /// Defines an image with meta data like <see cref="Keywords"/>,
    /// <see cref="Location"/>, etc.
    /// </summary>
    [Serializable]
    public class ImageWithMetadata
    {
        #region Private Variables

        private EpifaunalSubstrateScore epifaunalScore = new EpifaunalSubstrateScore();

        private BankStabilityScore bankScore = new BankStabilityScore();

        private List<Keyword> keywords;

        #endregion

        /// <summary>
        /// Represents an image as an array of bytes.
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Returns the <see cref="Keywords"/> as a 
        /// <see cref="System.Collections.ObjectModel.ReadOnlyCollection{Keyword}"/>
        /// </summary>
        public IList<Keyword> Keywords { get { return keywords.AsReadOnly(); } }

        /// <summary>
        /// Represents the area in which the image was taken.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Returns <see cref="EpifaunalSubstrateScore.Score"/> as an <see cref="int"/>
        /// </summary>
        public int EpifaunalSubstrateScore { get { return epifaunalScore.Score; } }

        /// <summary>
        /// Returns <see cref="BankStabilityScore.Score"/> as an <see cref="int"/>
        /// </summary>
        public int BankStabilityScore { get { return bankScore.Score; } }

        // Default Constructor
        private ImageWithMetadata()
        {
            this.keywords = new List<Keyword>();
        }

        // Constructor that takes in data as byte array
        public ImageWithMetadata(byte[] Data) : this()
        {
            this.Data = Data;
        }

        /// <summary>
        /// Changes the <see cref="EpifaunalSubstrateScore.Score"/>.
        /// </summary>
        /// <param name="i"></param>
        public void ChangeEpifaunalSubstrateScore(int i)
        {
            epifaunalScore.ChangeScore(i);
        }

        /// <summary>
        /// Changes the <see cref="BankStabilityScore.Score"/>.
        /// </summary>
        /// <param name="i"></param>
        public void ChangeBankStabilityScore(int i)
        {
            bankScore.ChangeScore(i);
        }

        /// <summary>
        /// Returns the size of the <see cref="ImageWithMetadata"/>.
        /// </summary>
        /// <returns></returns>
        public int GetImageSize()
        {
            return Data.Length;
        }

        /// <summary>
        /// Adds <see cref="Keyword"/> to <see cref="ImageWithMetadata"/>.
        /// </summary>
        /// <param name="keyword"></param>
        public void AddKeyword(Keyword keyword)
        {
            if (!keywords.Contains(keyword))
            {
                keywords.Add(keyword);
            }
        }
    }
}
