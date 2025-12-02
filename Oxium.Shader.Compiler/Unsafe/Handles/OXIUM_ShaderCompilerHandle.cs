using System;
using System.Collections.Generic;
using System.Text;

namespace Oxium.Shader.Unsafe.Handles
{
	public readonly partial struct OXIUM_ShaderCompilerHandle : IEquatable<OXIUM_ShaderCompilerHandle>
	{
		public OXIUM_ShaderCompilerHandle(nint handle) { Handle = handle; }
		public nint Handle { get; }
		public bool IsNull => Handle == 0;
		public static OXIUM_ShaderCompilerHandle Null => new OXIUM_ShaderCompilerHandle(0);
		public static implicit operator OXIUM_ShaderCompilerHandle(nint handle) => new OXIUM_ShaderCompilerHandle(handle);
		public static bool operator ==(OXIUM_ShaderCompilerHandle left, OXIUM_ShaderCompilerHandle right) => left.Handle == right.Handle;
		public static bool operator !=(OXIUM_ShaderCompilerHandle left, OXIUM_ShaderCompilerHandle right) => left.Handle != right.Handle;
		public static bool operator ==(OXIUM_ShaderCompilerHandle left, nint right) => left.Handle == right;
		public static bool operator !=(OXIUM_ShaderCompilerHandle left, nint right) => left.Handle != right;
		public bool Equals(OXIUM_ShaderCompilerHandle other) => Handle == other.Handle;
		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is OXIUM_ShaderCompilerHandle handle && Equals(handle);
		/// <inheritdoc/>
		public override int GetHashCode() => Handle.GetHashCode();
	}
}
