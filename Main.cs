using AMDaemon;
using NekoClient.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace UnityParrot
{
    //Main Entry Point
    public class Main
    {
        public static Components.XInputHandler InputHandler;

        static void InitPatches()
        {
            Components.OperationManagerPatches.Patch();

            Components.InitAimePatches.Patch();
            Components.AimeUnitPatches.Patch();
            Components.AimeResultPatches.Patch();
            Components.AccessCodePatches.Patch();
            Components.AimeIdPatches.Patch();

            Components.AMDaemonPatches.Patch();
            Components.AMManagerPatches.Patch();
            Components.BackupSettingPatches.Patch();
            Components.BookkeepPatches.Patch();
            Components.CreditPatches.Patch();
            Components.JvsPatches.Patch();
            Components.SequenceInitializePatches.Patch();
            Components.SerialPatches.Patch();
            Components.SysConfigPatches.Patch();
        }

        public static void Initialize()
        {
            Log.Initialize(LogLevel.All);
            Log.AddListener(new ConsoleLogListener());
            Log.AddListener(new TraceLogListener());
            Log.AddListener(new FileLogListener(FileSystem.Configuration.GetFilePath("..\\UnityParrot.log"), true));

            InitPatches();

            new Thread(() =>
            {
                Thread.Sleep(500);

                GameObject mainObject = new GameObject();

                InputHandler = mainObject.AddComponent<Components.XInputHandler>();
                mainObject.AddComponent<Components.SettingsManager>();
                mainObject.AddComponent<Components.AimePatches>();
                mainObject.AddComponent<Components.PacketPatches>();

                UnityEngine.Object.DontDestroyOnLoad(mainObject);

            }).Start();
        }
    }
}