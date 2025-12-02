using Oxium.Shader.Unsafe;
using Oxium.Shader.Unsafe.Handles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Oxium.Shader.Unsafe
{
    public unsafe static partial class OXIUM_ShaderCompiler
    {

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial OXIUM_ShaderCompilerHandle oxCreateShaderCompiler(delegate* unmanaged[Cdecl]<byte*,void*, void> log_callback,void* userData);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial OXIUM_SessionHandle oxCreateSession(OXIUM_ShaderCompilerHandle compiler, SessionDesc* desc);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial void* oxGetSessionUserData(OXIUM_SessionHandle session);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial OXIUM_ModuleHandle oxLoadModule(OXIUM_SessionHandle session,byte* moduleName, byte* path, byte* source);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial OXIUM_LinkingResult oxLinkModule(OXIUM_ModuleHandle module);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial byte* oxGetResultData(OXIUM_LinkingResult resultData,nint* size);

		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial void oxDestroyModule(OXIUM_ModuleHandle module);
		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial void oxDestroyResultData(nint resultData);
		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial void oxDestroySession(OXIUM_SessionHandle session);
		[LibraryImport("Oxium_ShaderCompiler")]
		public static partial void oxDestroyShaderCompiler(OXIUM_ShaderCompilerHandle compiler);

	}
}
