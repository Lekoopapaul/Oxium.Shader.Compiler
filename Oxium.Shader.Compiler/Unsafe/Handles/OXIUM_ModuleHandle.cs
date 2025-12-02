using System;
using System.Collections.Generic;
using System.Text;

namespace Oxium.Shader.Unsafe.Handles
{
	public readonly partial struct OXIUM_ModuleHandle : IEquatable<OXIUM_ModuleHandle>
	{
		public OXIUM_ModuleHandle(nint handle) { Handle = handle; }
		public nint Handle { get; }
		public bool IsNull => Handle == 0;
		public static OXIUM_ModuleHandle Null => new OXIUM_ModuleHandle(0);
		public static implicit operator OXIUM_ModuleHandle(nint handle) => new OXIUM_ModuleHandle(handle);
		public static bool operator ==(OXIUM_ModuleHandle left, OXIUM_ModuleHandle right) => left.Handle == right.Handle;
		public static bool operator !=(OXIUM_ModuleHandle left, OXIUM_ModuleHandle right) => left.Handle != right.Handle;
		public static bool operator ==(OXIUM_ModuleHandle left, nint right) => left.Handle == right;
		public static bool operator !=(OXIUM_ModuleHandle left, nint right) => left.Handle != right;
		public bool Equals(OXIUM_ModuleHandle other) => Handle == other.Handle;
		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is OXIUM_ModuleHandle handle && Equals(handle);
		/// <inheritdoc/>
		public override int GetHashCode() => Handle.GetHashCode();
	}
}
