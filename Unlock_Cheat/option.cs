using System;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using KMod;
using Klei;

namespace Unlock_Cheat
{

    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("", null, false)]
    [ConfigFile("config.json", true, true)]
    [RestartRequired]
    internal  sealed class Options : SingletonOptions<Options>
    {

        [JsonProperty]
        [Option("成就解锁", "debug 沙盒能解锁成就.", null)]
        public bool Achievement { get; set; }

        [JsonProperty]
        [Option("皮肤解锁", "解锁全皮肤.", null)]
        public bool Skin { get; set; }

        [JsonProperty]
        [Option("管道相变", "禁止气液管道内容物相变.", null)]
        public bool Conduit { get; set; }

        [JsonProperty]
        [Option("植物变异", "种子/植物添加变异按钮.", null)]
        public bool MutantPlant { get; set; }

        [JsonProperty]
        [Option("植物自动收获按钮修改", "植物禁用自动收获时,附加一个自动收获状态.之后不需要小人收获了", null)]
        public bool MutantPlant_SelfHarvest { get; set; }

        [JsonProperty]
        [Option("植物自动收获单独按钮", "单独附加一个自动收获按钮,还原自动收获按钮功能", null)]
        public bool MutantPlant_SelfHarvest_Independent { get; set; }


        public Options()
        {
            this.Achievement = true;
            this.Skin = true;
            this.Conduit = true;
            this.MutantPlant = true;
            this.MutantPlant_SelfHarvest= true;
            this.MutantPlant_SelfHarvest_Independent = false;
        }
    }
}
