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
        [Option("皮肤解锁", "解锁全皮肤!!!请勿宣传!!!", null)]
        public bool Skin { get; set; }

        [JsonProperty]
        [Option("管道相变", "禁止气液管道内容物相变.", null)]
        public bool Conduit { get; set; }

        [JsonProperty]
        [Option("植物变异", "种子/植物添加变异按钮.", null)]
        public bool MutantPlant { get; set; }

        [JsonProperty]
        [Option("允许植物多次变异", "变异植物添加也变异按钮,选择好需要的变异后.重新保存读档只有最后一次变异才会生效.否则面板上的数据不准确,实际上是多个变异效果叠加,会有意料之外的效果", null)]
        public bool MutantPlant_Mult { get; set; }

        [JsonProperty]
        [Option("启用(功能修改)", "修改到游戏内植物原来的自动收获按钮，植物禁用自动收获时,附加一个自动收获状态.之后不需要小人收获了", "植物自动收获(二选一)")]
        public bool MutantPlant_SelfHarvest { get; set; }

        [JsonProperty]
        [Option("启用(单独按钮)", "单独生成一个自动收获按钮,植物原本的自动收获按钮功能不变,优先级高于上面", "植物自动收获(二选一)")]
        public bool MutantPlant_SelfHarvest_Independent { get; set; }


        public Options()
        {
            this.Achievement = true;
            this.Skin = false;
            this.Conduit = true;
            this.MutantPlant = true;
            this.MutantPlant_Mult = false;
            this.MutantPlant_SelfHarvest= true;
            this.MutantPlant_SelfHarvest_Independent = false;
        }
    }
}
