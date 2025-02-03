using System.Reflection;
using UnityEngine;

namespace PlayerUpgradeStats.Integrations
{
    internal static class HotKeyCommandsIntegration
    {
        internal static void Setup()
        {
            LoadHotKeyCommandsDllIfFound();
            AddSUIElement(PlayerUpgradeStatsUi.MOD_LIST_ID);
        }

        private static bool alreadyLoaded = false;
        private static Assembly hotKeyCommandsAssembly;
        private static Type suiuiType;
        private static Type unityUiType;

        public static void LoadHotKeyCommandsDllIfFound()
        {
            if (alreadyLoaded)
            {
                PlayerStatsFunctions.PostMessage("HotKeyCommands DLL should already be found and loaded, returning");
                return;
            }
            alreadyLoaded = true;

            // Define the path to the DLL
            string executingAssemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string parentDirectory = Directory.GetParent(executingAssemblyDirectory).FullName;
            string dllPath = Path.Combine(parentDirectory, "Mods", "HotKeyCommands.dll");

            PlayerStatsFunctions.PostMessage($"Dll Path: {dllPath}");

            if (File.Exists(dllPath))
            {
                try
                {
                    // Load the DLL
                    hotKeyCommandsAssembly = Assembly.LoadFrom(dllPath);
                    PlayerStatsFunctions.PostMessage("HotKeyCommands.dll found and loaded.");

                    // Get the SUIUI type
                    suiuiType = hotKeyCommandsAssembly.GetType("HotKeyCommands.SUIUI");
                    unityUiType = hotKeyCommandsAssembly.GetType("HotKeyCommands.UnityUi");

                    if (suiuiType != null)
                    {
                        PlayerStatsFunctions.PostMessage("HotKeyCommands.SUIUI type found.");
                    }
                    else
                    {
                        PlayerStatsFunctions.PostMessage("HotKeyCommands.SUIUI type not found.");
                    }

                    if (unityUiType != null)
                    {
                        PlayerStatsFunctions.PostMessage("HotKeyCommands.UnityUi type found.");
                    }
                    else
                    {
                        PlayerStatsFunctions.PostMessage("HotKeyCommands.UnityUi type not found.");
                    }
                }
                catch (Exception ex)
                {
                    PlayerStatsFunctions.PostMessage($"Failed to load HotKeyCommands.dll: {ex.Message}");
                }
            }
            else
            {
                PlayerStatsFunctions.PostMessage("HotKeyCommands.dll not found.");
            }
        }

        public static void AddSUIElement(string element)
        {
            if (suiuiType == null)
            {
                PlayerStatsFunctions.PostMessage("SUIUI type is not loaded.");
                return;
            }

            try
            {
                MethodInfo addMethod = suiuiType.GetMethod("AddSUIElemet", BindingFlags.Static | BindingFlags.Public);
                if (addMethod != null)
                {
                    addMethod.Invoke(null, new object[] { element });
                    PlayerStatsFunctions.PostMessage($"Element '{element}' added.");
                }
                else
                {
                    PlayerStatsFunctions.PostMessage("AddSUIElemet method not found.");
                }
            }
            catch (Exception ex)
            {
                PlayerStatsFunctions.PostMessage($"Error invoking AddSUIElemet: {ex.Message}");
            }
        }

        public static void RemoveSUIElement(string element)
        {
            if (suiuiType == null)
            {
                PlayerStatsFunctions.PostMessage("SUIUI type is not loaded.");
                return;
            }
            try
            {
                MethodInfo removeMethod = suiuiType.GetMethod("RemoveSUIElemet", BindingFlags.Static | BindingFlags.Public);
                if (removeMethod != null)
                {
                    removeMethod.Invoke(null, new object[] { element });
                    PlayerStatsFunctions.PostMessage($"Element '{element}' removed.");
                }
                else
                {
                    PlayerStatsFunctions.PostMessage("RemoveSUIElemet method not found.");
                }
            }
            catch (Exception ex)
            {
                PlayerStatsFunctions.PostMessage($"Error invoking RemoveSUIElemet: {ex.Message}");
            }
        }

        public static void AddUnityElement(GameObject unityElement)
        {
            if (unityUiType == null)
            {
                PlayerStatsFunctions.PostMessage("UnityUi type is not loaded.");
                return;
            }

            try
            {
                MethodInfo addMethod = unityUiType.GetMethod("AddUnityElement", BindingFlags.Static | BindingFlags.Public);
                if (addMethod != null)
                {
                    addMethod.Invoke(null, new object[] { unityElement });
                    PlayerStatsFunctions.PostMessage($"Unity element added.");
                }
                else
                {
                    PlayerStatsFunctions.PostMessage("AddUnityElement method not found.");
                }
            }
            catch (Exception ex)
            {
                PlayerStatsFunctions.PostMessage($"Error invoking AddUnityElement: {ex.Message}");
            }
        }

        public static void RemoveUnityElement(GameObject unityElement)
        {
            if (unityUiType == null)
            {
                PlayerStatsFunctions.PostMessage("UnityUi type is not loaded.");
                return;
            }

            try
            {
                MethodInfo removeMethod = unityUiType.GetMethod("RemoveUnityElement", BindingFlags.Static | BindingFlags.Public);
                if (removeMethod != null)
                {
                    removeMethod.Invoke(null, new object[] { unityElement });
                    PlayerStatsFunctions.PostMessage($"Unity element removed.");
                }
                else
                {
                    PlayerStatsFunctions.PostMessage("RemoveUnityElement method not found.");
                }
            }
            catch (Exception ex)
            {
                PlayerStatsFunctions.PostMessage($"Error invoking RemoveUnityElement: {ex.Message}");
            }
        }
    }
}
