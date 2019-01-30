using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;

namespace StreamBED.Backend.Helper
{
    /// <summary>
    /// Defines methods to serialize and deserialize a list of <see cref="ImageWithMetadata"/>.
    /// </summary>
    [Serializable]
    public class ImageSerialization
    {
        // Stores all the images that are going to be serialized or deserialized.
        private List<ImageWithMetadata> imageList;

        /// <summary>
        /// Returns the <see cref="ImageSerialization.imageList"/> as a 
        /// <see cref="System.Collections.ObjectModel.ReadOnlyCollection{ImageWithMetadata}"/>
        /// </summary>
        public IList<ImageWithMetadata> ImageList { get { return imageList.AsReadOnly(); } }

        // Constructor
        public ImageSerialization()
        {
            imageList = new List<ImageWithMetadata>();
        }

        /// <summary>
        /// Adds an <see cref="ImageWithMetadata"/> into 
        /// ImageList to be serialized or deserialized.
        /// </summary>
        /// <param name="Image"><see cref="ImageWithMetadata"/> to be added.</param>
        public void AddImage(ImageWithMetadata Image)
        {
            imageList.Add(Image);
        }

        /// <summary>
        /// Removes an <see cref="ImageWithMetadata"/> from <paramref name="ImageList"/>.
        /// </summary>
        /// <param name="Image"><see cref="ImageWithMetadata"/> to be removed.</param>
        public void RemoveImage(ImageWithMetadata Image)
        {
            imageList.Remove(Image);
        }

        /// <summary>
        /// Serializes ImageList and is saved to ImageData.dat.
        /// </summary>
        public void SerializeImage()
        {
            Stream stream = File.Open("ImageData.dat", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(stream, ImageList);

            stream.Close();
        }

        /// <summary>
        /// Deserializes ImageList from ImageData.dat.
        /// </summary>
        public void DeserializeImage()
        {
            Stream stream = File.Open("ImageData.dat", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            imageList = (List<ImageWithMetadata>)binaryFormatter.Deserialize(stream);

            stream.Close();
        }
    }
}
