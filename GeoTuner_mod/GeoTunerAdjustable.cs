using HarmonyLib;
using KSerialization;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;

using UnityEngine;

using static UnityEngine.UI.Image;

namespace GeoTuner_mod
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class GeoTunerAdjustable : KMonoBehaviour, IUserControlledCapacity
    {
   

        private static Options option = SingletonOptions<Options>.Instance;

        private static void OnCopySettings(GeoTunerAdjustable comp, object data)
        {
            comp.OnCopySettings(data);
        }

        public float UserMaxCapacity
        {
            get
            {
                return Math.Min(this.MAX_GEOTUNED, option.Maxcount) ;
            }
            set
            {
                this.MAX_GEOTUNED = value;
            }
        }

        public float AmountStored
        {
            get
            {
                return this.UserMaxCapacity;
            }
        }


        public float MinCapacity
        {
            get
            {
                return 1f;
            }
        }


        public float MaxCapacity
        {
            get
            {
                return option.Maxcount;
            }
        }


        public bool WholeValues
        {
            get
            {
                return false;
            }
        }

        public LocString CapacityUnits
        {
            get
            {
                return UI.UISIDESCREENS.GEOTUNERADJUSTABLE.UNITS;
            }
        }

        //public int SliderDecimalPlaces(int i)
        //{
        //    return 0;
        //}

        //public float GetSliderValue(int i)
        //{
        //    return this.MAX_GEOTUNED;
        //}

        //public string GetSliderTooltipKey(int i)
        //{
        //    return "GeoTuner_mod.UI.UISIDESCREENS.GeoTunerAdjustable.TOOLTIP";
        //}


        //public string GetSliderTooltip(int i)
        //{
        //    return string.Format(UI.UISIDESCREENS.GeoTunerAdjustable.TOOLTIP, this.MAX_GEOTUNED);
        //}


        //public string SliderTitleKey
        //{
        //    get
        //    {
        //        return "GeoTuner_mod.UI.UISIDESCREENS.GeoTunerAdjustable.TITLE";
        //    }
        //}


        //public string SliderUnits
        //{
        //    get
        //    {
        //        return UI.UISIDESCREENS.GeoTunerAdjustable.Units;
        //    }
        //}

        //public void SetSliderValue(float val, int i)
        //{
        //    if(val == Old_GEOTUNED)
        //    {
        //        return;
        //    }
        //    this.MAX_GEOTUNED = val;
        //    this.Update();
        //}


        //public float GetSliderMin(int index)
        //{
        //    return 1f;
        //}

        //public float GetSliderMax(int index)
        //{
        //    return option.Maxcount;
        //}

        protected override void OnPrefabInit()
        {
         
            base.OnPrefabInit();

            base.Subscribe<GeoTunerAdjustable>(-905833192, GeoTunerAdjustable.OnCopySettingsDelegate);
        }

 
        protected override void OnSpawn()
        {

            if (option.energyConsumer)
            {
                this.BaseWattageRating = this.energyConsumer.WattsNeededWhenActive;
                this.energyConsumer.BaseWattageRating = this.BaseWattageRating * this.MAX_GEOTUNED * option.Geotuners_Ratio;
            }


            this.Update();
        }

 
        internal void OnCopySettings(object data)
        {
            GeoTunerAdjustable component = ((GameObject)data).GetComponent<GeoTunerAdjustable>();
            bool flag = component != null;
            if (flag)
            {
                this.MAX_GEOTUNED = component.MAX_GEOTUNED;
            }
        }

    
        internal void Update()
        {

            bool flag = this.MAX_GEOTUNED != Old_GEOTUNED;

            if ( !flag)
            {

                return;
            }
            GeoTuner.Instance targetGeotuner = base.gameObject.GetSMI<GeoTuner.Instance>();

            if (targetGeotuner == null)
            {
                return;
            }
            Geyser assignedGeyser = targetGeotuner.GetFutureGeyser();


            if(assignedGeyser == null)
            { return; }
  
            if (flag)
            {
               StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter FutureGeyser = Traverse.Create(targetGeotuner.sm).Field("FutureGeyser").GetValue<StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter>();
                //StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter DropStorage = Traverse.Create(targetGeotuner.sm).Field("DropStorageIfNotMatching").GetValue<StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.TargetParameter>();
               // if (FutureGeyser == null) { return; }
                FutureGeyser.Set(null, targetGeotuner);
                targetGeotuner.AssignGeyser(null);

                //Chore switchGeyserChore = Traverse.Create(targetGeotuner.sm).Field("switchGeyserChore").GetValue<Chore>();

                //if (switchGeyserChore != null)
                //{
                //    Chore chore = switchGeyserChore;
                //    Traverse.Create(chore).Field("isComplete").SetValue(true);
                //    global::Debug.Log("协调：" + chore.isComplete);

                //    Traverse.Create(targetGeotuner).Method("AbortSwitchGeyserChore", new System.Type[] { typeof(String) }).GetValue();
                //    Traverse.Create(targetGeotuner).Method("OnSwitchGeyserChoreCompleted", new System.Type[] { typeof(Chore) }).GetValue(new object[] { chore });
                //}



                //targetGeotuner.storage.DropAll(false, false, default(Vector3), true, null);
                //Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings> geotunedGeyserSettings1 = new Dictionary<HashedString, GeoTunerConfig.GeotunedGeyserSettings>();
                //GeoTunerConfig.GeotunedGeyserSettings value;
                //if (!geotunerGeyserSettings.TryGetValue(assignedGeyser.configuration.typeId, out value))
                //{
                //    DebugUtil.DevLogError(string.Format("Geyser {0} is missing a Geotuner setting, using default", assignedGeyser.configuration.typeId));
                //    value = targetGeotuner.def.defaultSetting;
                //}
                //value.quantity = value.quantity * MAX_GEOTUNED * option.Geotuners_Ratio;
                //value.template.massPerCycleModifier = value.template.massPerCycleModifier *MAX_GEOTUNED * option.Geyser_Ratio;
                //value.template.temperatureModifier = value.template.temperatureModifier * MAX_GEOTUNED * option.Geyser_Ratio;
                //global::Debug.Log("协调：" + value.quantity);


                //foreach (var pair in geotunerGeyserSettings)
                //{

                //    geotunedGeyserSettings1.Add(pair.Key, pair.Value);
                //}
                //geotunedGeyserSettings1[assignedGeyser.configuration.typeId] = value;

                //targetGeotuner.def.geotunedGeyserSettings = geotunedGeyserSettings1;
                //targetGeotuner.AssignFutureGeyser(assignedGeyser);


                if (assignedGeyser != null)
                {
                    Traverse.Create(typeof(GeoTuner)).Method("RemoveTuning", new System.Type[] { typeof(GeoTuner.Instance) }).GetValue(new object[] { targetGeotuner });
                    assignedGeyser.Unsubscribe(-593169791, new Action<object>(targetGeotuner.OnEruptionStateChanged));
                }
                Geyser geyser2 = assignedGeyser;
                Traverse.Create(targetGeotuner).Method("RefreshModification").GetValue();
      
                if (assignedGeyser != null)
                {
                    assignedGeyser.Subscribe(-593169791, new Action<object>(targetGeotuner.OnEruptionStateChanged));
                    assignedGeyser.Trigger(1763323737, null);
                }
                StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Signal geyserSwitchSignal = Traverse.Create(targetGeotuner.sm).Field("geyserSwitchSignal").GetValue<StateMachine<GeoTuner, GeoTuner.Instance, IStateMachineTarget, GeoTuner.Def>.Signal>();

                geyserSwitchSignal.Trigger(targetGeotuner);


                targetGeotuner.AssignFutureGeyser(assignedGeyser);

                if (option.energyConsumer)
                { this.energyConsumer.BaseWattageRating = this.BaseWattageRating * this.MAX_GEOTUNED * option.Geotuners_Ratio; }

                Old_GEOTUNED = this.MAX_GEOTUNED;
                //global::Debug.Log("协调：" + this.MAX_GEOTUNED);
            }
            //targetGeotuner.AssignGeyser(assignedGeyser);

        }
               

            
 


        private static readonly EventSystem.IntraObjectHandler<GeoTunerAdjustable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<GeoTunerAdjustable>(new Action<GeoTunerAdjustable, object>(GeoTunerAdjustable.OnCopySettings));



        //[SerializeField]
        //private ToolTip toolTip;

        [MyCmpAdd]
        public CopyBuildingSettings copyBuildingSettings;


        private float BaseWattageRating;

        [Serialize]
        private float Old_GEOTUNED = 1f;

        [MyCmpReq]
        public EnergyConsumer energyConsumer;

        [Serialize]
        private float MAX_GEOTUNED = 1f;
    }
}
