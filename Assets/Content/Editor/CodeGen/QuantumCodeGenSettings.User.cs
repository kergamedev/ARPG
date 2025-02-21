namespace Quantum.Editor
{
    using Quantum.CodeGen;

    public static partial class QuantumCodeGenSettings
    {
        static partial void GetCodeGenFolderPathUser(ref string path)
        {
            path = "Assets/Content/Simulation/Generated";
        }

        static partial void GetCodeGenUnityRuntimeFolderPathUser(ref string path)
        {
            path = "Assets/Content/View/Generated";
        }

        static partial void GetOptionsUser(ref GeneratorOptions options) { }
    }
}