using System;
using System.Collections.Generic;
using System.Text;

namespace Oxium.Shader.Unsafe.Handles
{
	public readonly partial struct OXIUM_SessionHandle : IEquatable<OXIUM_SessionHandle>
	{
		public OXIUM_SessionHandle(nint handle) { Handle = handle; }
		public nint Handle { get; }
		public bool IsNull => Handle == 0;
		public static OXIUM_SessionHandle Null => new OXIUM_SessionHandle(0);
		public static implicit operator OXIUM_SessionHandle(nint handle) => new OXIUM_SessionHandle(handle);
		public static bool operator ==(OXIUM_SessionHandle left, OXIUM_SessionHandle right) => left.Handle == right.Handle;
		public static bool operator !=(OXIUM_SessionHandle left, OXIUM_SessionHandle right) => left.Handle != right.Handle;
		public static bool operator ==(OXIUM_SessionHandle left, nint right) => left.Handle == right;
		public static bool operator !=(OXIUM_SessionHandle left, nint right) => left.Handle != right;
		public bool Equals(OXIUM_SessionHandle other) => Handle == other.Handle;
		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is OXIUM_SessionHandle handle && Equals(handle);
		/// <inheritdoc/>
		public override int GetHashCode() => Handle.GetHashCode();
	}
}
