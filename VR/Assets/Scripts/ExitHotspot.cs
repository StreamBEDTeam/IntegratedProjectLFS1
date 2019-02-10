using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StreamBED.Backend.Helper;
using StreamBED.Backend.Models.ProtocolModels;
using System.IO;
public class ExitHotspot : Hotspot
{
    public override void HotspotTrigger()
    {
        SerializePhotos();
        SceneManager.LoadScene("MenuScene");
    }

    public void SerializePhotos()
    {
        //Map feature name in Unity to Keyword in Backend
        var keywordMap = new Dictionary<string, Keyword>();



        keywordMap["CobbleNone"] = null;
        keywordMap["CobbleSome"] = EpifaunalSubstrateModel.Keywords.Cobble;

        keywordMap["SnagsNone"] = null;
        keywordMap["SnagsSome"] = EpifaunalSubstrateModel.Keywords.SnagsLogs;

        keywordMap["WaterVegNone"] = null;
        keywordMap["WaterVegSome"] = EpifaunalSubstrateModel.Keywords.UnderwaterVegetation;

        keywordMap["UndercutNone"] = null;
        keywordMap["UndercutSome"] = EpifaunalSubstrateModel.Keywords.UndercutBanks;

        keywordMap["SlopeSteep"] = BankStabilityModel.Keywords.BankSlope;
        keywordMap["SlopeMedium"] = BankStabilityModel.Keywords.BankSlope;
        keywordMap["SlopeGentle"] = BankStabilityModel.Keywords.BankSlope;

        keywordMap["BankVegNone"] = null;
        keywordMap["BankVegGrass"] = BankStabilityModel.Keywords.BankVegetation;
        keywordMap["BankVegPlants"] = BankStabilityModel.Keywords.BankVegetation;

        keywordMap["RootsNone"] = null;
        keywordMap["RootsSome"] = BankStabilityModel.Keywords.ExposedTreeRoots;

        keywordMap["CrumblingNone"] = null;
        keywordMap["CrumblingSome"] = BankStabilityModel.Keywords.BankFailure;




     //   keywordMap["ErodedSome"] = BankStabilityModel.Keywords.ErodedAreas;
        //keywordMap[" "] = BankStabilityModel.Keywords.ExposedTreeRoots;
        //keywordMap[" "] = BankStabilityModel.Keywords.BankFailure;

        //keywordMap[""]

        /*
         * EpifaunalSubstrateModel
        public static readonly Keyword Snags;
        public static readonly Keyword SubmergedLogs;
        public static readonly Keyword UndercutBanks;
        public static readonly Keyword Cobble;

        BankStabilityModel
    public static readonly Keyword SteepBank;
    public static readonly Keyword GentlySlopingBank;
    public static readonly Keyword BankFailure;
    public static readonly Keyword CrumblingBank;
    public static readonly Keyword ErosionalScars;
    public static readonly Keyword ExposedSoil;
    public static readonly Keyword ExposedTreeRoot;
    */

        var handle = GameObject.FindObjectOfType<GameStateHandle>();
        var state = handle.Instance;

        var ser = new ImageSerialization();
        foreach(var photo in state.Photos)
        {
            var bytes = File.ReadAllBytes(photo.ImagePath);
            var image = new ImageWithMetadata(bytes);
            image.Location = photo.AreaName;
            foreach(var tag in photo.Tags)
            {
                if (keywordMap.ContainsKey(tag))
                {
                    image.AddKeyword(keywordMap[tag]);
                }
                else
                {
                    Debug.LogErrorFormat("Missing keyword in keywordMap: [{0}]", tag);
                }
            }
            ser.AddImage(image);
        }
        try
        {
            ser.SerializeImage();
        }
        catch (System.Runtime.Serialization.SerializationException ex)
        {
            Debug.LogException(ex);
        }
    }
}
