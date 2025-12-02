using Oxium.Shader.Unsafe;
using Oxium.Shader.Unsafe.Handles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Oxium.Shader
{
	public class Module : IDisposable
	{
		private bool _disposedValue;
		private OXIUM_ModuleHandle _handle;

		internal Module(OXIUM_ModuleHandle handle) 
		{
			_handle = handle;
		}

		public byte[]? Link()
		{
			var result = OXIUM_ShaderCompiler.oxLinkModule(_handle);
			if(result.IsNull)
				return null;
			nint size = 0;
			unsafe
			{
				var ptr = OXIUM_ShaderCompiler.oxGetResultData(result, &size);
				byte[] ret = new byte[size];
				Marshal.Copy((nint)ptr, ret, 0, (int)size);
				return ret;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{

				OXIUM_ShaderCompiler.oxDestroyModule(_handle);
				_disposedValue = true;
			}
		}

		~Module()
			=> Dispose(false);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
