// Human-readable equivalent of the tiny bootstrap emitted by tools/build.py.
// The release DLL embeds FoATrainerRuntime.cs as a UTF-16 user string and
// compiles it inside the game with the game's own Mono.CSharp evaluator.

using System;
using System.IO;
using System.Reflection;
using BepInEx;

namespace FoATrainer
{
    [BepInPlugin("rijiy.foa.trainer.v17_1", "Tainted Grail Trainer by Rijiy V17.1", "2.5.1")]
    public class Bootstrap : BaseUnityPlugin
    {
        private const string BootLog = @"BepInEx\FoATrainer_boot.log";
        private const string CompileLog = @"BepInEx\FoATrainer_compile.log";

        private void Awake()
        {
            Log("[FoATrainer.V17.1] Awake entered");

            Assembly mcs = Assembly.Load("mcs");
            Log("[FoATrainer.V17.1] mcs assembly loaded");

            Type settingsType = mcs.GetType("Mono.CSharp.CompilerSettings");
            object settings = Activator.CreateInstance(settingsType);
            Log("[FoATrainer.V17.1] CompilerSettings created");

            StreamWriter writer = new StreamWriter(CompileLog);
            writer.AutoFlush = true;

            Type reportType = mcs.GetType("Mono.CSharp.StreamReportPrinter");
            object report = Activator.CreateInstance(reportType, new object[] { writer });
            Log("[FoATrainer.V17.1] StreamReportPrinter created");

            Type contextType = mcs.GetType("Mono.CSharp.CompilerContext");
            object context = Activator.CreateInstance(contextType, new object[] { settings, report });
            Log("[FoATrainer.V17.1] CompilerContext created");

            Type evaluatorType = mcs.GetType("Mono.CSharp.Evaluator");
            object evaluator = Activator.CreateInstance(evaluatorType, new object[] { context });
            Log("[FoATrainer.V17.1] Evaluator created");

            MethodInfo referenceAssembly = evaluatorType.GetMethod("ReferenceAssembly");
            string[] references = {
                "System", "System.Core", "UnityEngine", "UnityEngine.CoreModule",
                "UnityEngine.IMGUIModule", "UnityEngine.InputLegacyModule",
                "UnityEngine.TextRenderingModule", "0Harmony", "BepInEx"
            };

            foreach (string name in references)
            {
                Log("[FoATrainer.V17.1] Referencing " + name);
                referenceAssembly.Invoke(evaluator, new object[] { Assembly.Load(name) });
                Log("[FoATrainer.V17.1] Reference OK " + name);
            }

            // In the release DLL this string is embedded by tools/build.py.
            string runtimeSource = LoadEmbeddedRuntimeSource();
            MethodInfo run = evaluatorType.GetMethod("Run");

            Log("[FoATrainer.V17.1] Compiling runtime source");
            run.Invoke(evaluator, new object[] { runtimeSource });

            Log("[FoATrainer.V17.1] Starting runtime");
            run.Invoke(evaluator, new object[] { "FoATrainerRuntime.Start();" });
            Log("[FoATrainer.V17.1] Awake completed");
        }

        private static string LoadEmbeddedRuntimeSource()
        {
            // Reference implementation only. tools/build.py embeds the contents
            // directly into the generated DLL, so the shipped bootstrap does not
            // need a file next to it.
            string local = Path.Combine(Paths.PluginPath, "FoATrainerRuntime.cs");
            return File.ReadAllText(local);
        }

        private static void Log(string text)
        {
            File.AppendAllText(BootLog, text + Environment.NewLine);
        }
    }
}
