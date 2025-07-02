using System.Collections.Generic;
#if !UNITY_EDITOR
using Obfuz;
using Obfuz.EncryptionVM;
#endif
using UnityEngine;

namespace EscapeGame.Core.Manager
{

    public static class ProcedureBoot
    {
        private static ProcedureFSM fsm = new ProcedureFSM();
        
        //备注：为什么用AfterSceneLoad，避免BattleRoom场景上StageCamera脚本多创建一个UICamera,避免GameObject.Find找不到对象
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void BootGame()
        {
#if !UNITY_EDITOR
            EncryptionService<DefaultStaticEncryptionScope>.Encryptor = new GeneratedEncryptionVirtualMachine(Resources.Load<TextAsset>("Obfuz/defaultStaticSecretKey").bytes);
#endif
            //---------------------battleRoom-----------------------------
            //设置对应的Transition
            //fsm.AddTransition<Procedure_PreBoot, Procedure_BootPipelineInBattleRoom>();

            ////---------------------Normal-----------------------------
            //fsm.AddTransition<Procedure_PreBoot, Procedure_BootPipelineInStable>();
            //fsm.AddTransition<Procedure_BootPipelineInStable, Procedure_PrepareEnterGame>();
            //fsm.AddTransition<Procedure_PrepareEnterGame, Procedure_EnterGame>();
            //fsm.AddTransition<Procedure_ExitGame, Procedure_BootPipelineInStable>();


            //fsm.AddTwoWayTransition<Procedure_EnterGame, Procedure_ExitGame>();
            //fsm.AddTwoWayTransition<Procedure_EnterGame, Procedure_AttackPermanentPoint>();
            //fsm.AddTwoWayTransition<Procedure_EnterGame, Procedure_AttackTemporaryPoint>();
            //fsm.AddTwoWayTransition<Procedure_EnterGame, Procedure_DoActiveBackTrack>();
            //fsm.AddTwoWayTransition<Procedure_EnterGame, Procedure_DoDeathBackTrack>();

            ////难度选项部分会影响流程transition，增加死亡跳转到ExitGame
            //fsm.AddTwoWayTransition<Procedure_DoDeathBackTrack, Procedure_ExitGame>();

            ////设置初始流程
            //fsm.SetDefaultState<Procedure_PreBoot>();
        }

#if UNITY_EDITOR
        public static Dictionary<string, HashSet<string>> GetAllTransition()
        {
            return fsm.GetAllTransition();
        }
        public static string GetNowTransitionName()
        {
            return fsm.GetNowTransitionName();
        }
#endif
    }
}
