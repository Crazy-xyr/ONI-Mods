using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace GeoTuner_mod
{

    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("", null, false)]
    [ConfigFile("config.json", true, true)]
    [RestartRequired]
    internal class Options : SingletonOptions<Options>
    {


        [JsonProperty]
        [Option("协调最大值", "修改地质调谐仪最大次数", null)]
        [Limit(5,10)]
        public int  Maxcount { get; set; }




        [JsonProperty]
        [Option("调谐仪资源消耗倍率", "协调地质调谐仪消耗资源的倍率", null)]
        [Limit(0.1, 10)]
        public float Geotuners_Ratio { get; set; }


        [JsonProperty]
        [Option("间歇泉产出倍率", "协调间歇泉产出增加的倍率", null)]
        [Limit(0.1, 10)]
        public float Geyser_Ratio { get; set; }

        [JsonProperty]
        [Option("调谐仪原版限制", "地质调谐仪像原版一样同时调谐一个泉", null)]
        public bool Broker_Vanilla { get; set; }

        [JsonProperty]
        [Option("调谐仪耗电量", "地质调谐仪耗电量是否同步增加", null)]
        public bool energyConsumer { get; set; }


        public Options()
        {
            this.Maxcount = 5;
            this.Geotuners_Ratio = 1;
            this.Geyser_Ratio = 1;
            this.Broker_Vanilla = false;
            this.energyConsumer= true;

        }






    }
}
