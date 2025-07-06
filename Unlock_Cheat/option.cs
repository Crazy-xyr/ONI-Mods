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
        [Option("植物变异", "种子/植物添加变异按钮.", "植物变异")]
        public bool MutantPlant { get; set; }

        [JsonProperty]
        [Option("允许植物多次变异", "变异植物添加也变异按钮,选择好需要的变异后.重新保存读档只有最后一次变异才会生效.否则面板上的数据不准确,实际上是多个变异效果叠加,会有意料之外的效果", "植物变异")]
        public bool MutantPlant_Mult { get; set; }

        [JsonProperty]
        [Option("变异植物自动收获", "所有的变异植物都会自动收获按,需要开启植物变异", "植物变异")]
        public bool MutantPlant_SelfHarvest { get; set; }


        //[JsonProperty]
        //[Option("启用植物自动收获按钮", "单独生成一个自动收获按钮,需要开启植物变异", "植物变异")]
        public bool MutantPlant_SelfHarvest_Independent { get; set; }


        [JsonProperty]
        [Option("太空挖矿倍率", "用更少的钻石挖更多的矿", "太空挖矿")]
        [Limit(1f,1000f)]
        public float Harvest_mult { get; set; }

        [JsonProperty]
        [Option("火箭货仓容量修改", "挖矿效率提高应该搭配更大的货仓,装卸端口也会同步提高", "太空挖矿")]
        [Limit(1f, 1000f)]
        public float Harvest_storage_mult { get; set; }

        [JsonProperty]
        [Option("太空矿物质量修改", "挖矿效率提高应该搭配更大的矿物", "太空挖矿")]
        [Limit(1f, 1000f)]
        public float Harvest_poi_mult { get; set; }

        [JsonProperty]
        [Option("伤害修改", "单发火箭伤害", "宇宙内爆破弹")]
        [Limit(0, 1000)]
        public int MissileLongRange_damage { get; set; }


        public Options()
        {
            this.Achievement = true;
            this.Skin = false;
            this.Conduit = true;
            this.MutantPlant = true;
            this.MutantPlant_Mult = false;
            this.MutantPlant_SelfHarvest= false;
            this.MutantPlant_SelfHarvest_Independent = false;
            this.Harvest_mult = 1f;
            this.Harvest_storage_mult = 1f;
            this.Harvest_poi_mult = 1f;
            this.MissileLongRange_damage = 10;

        }
    }
}
