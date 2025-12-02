using System;
using System.Collections.Generic;
using System.Text;

namespace Oxium.Shader.Unsafe.Handles
{
	public readonly partial struct OXIUM_LinkingResult : IEquatable<OXIUM_LinkingResult>
	{
		public OXIUM_LinkingResult(nint handle) { Handle = handle; }
		public nint Handle { get; }
		public bool IsNull => Handle == 0;
		public static OXIUM_LinkingResult Null => new OXIUM_LinkingResult(0);
		public static implicit operator OXIUM_LinkingResult(nint handle) => new OXIUM_LinkingResult(handle);
		public static bool operator ==(OXIUM_LinkingResult left, OXIUM_LinkingResult right) => left.Handle == right.Handle;
		public static bool operator !=(OXIUM_LinkingResult left, OXIUM_LinkingResult right) => left.Handle != right.Handle;
		public static bool operator ==(OXIUM_LinkingResult left, nint right) => left.Handle == right;
		public static bool operator !=(OXIUM_LinkingResult left, nint right) => left.Handle != right;
		public bool Equals(OXIUM_LinkingResult other) => Handle == other.Handle;
		/// <inheritdoc/>
		public override bool Equals(object? obj) => obj is OXIUM_LinkingResult handle && Equals(handle);
		/// <inheritdoc/>
		public override int GetHashCode() => Handle.GetHashCode();
	}
}
