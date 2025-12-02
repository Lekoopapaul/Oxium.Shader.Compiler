using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Oxium.Shader.Unsafe
{
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct Defines
	{
		public byte* name;
		public byte* value;
	}
	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct SessionDesc
	{
		public Defines* macros;
		public int macroCount;
		public byte** searchPaths;
		public int searchPathCount;
		public int OptimizationLevel;
		/*!the slang target profile */
		public byte* profile;
		/*! can not be null*/
		public delegate* unmanaged[Cdecl]<byte*, nint*, void*, byte*> loadFile;
		/*! can not be null*/
		public delegate* unmanaged[Cdecl]<void*, void*, void> freePtr;
		public void* userData;
	}
}
