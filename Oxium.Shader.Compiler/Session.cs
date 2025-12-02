using Oxium.Shader.Unsafe;
using Oxium.Shader.Unsafe.Handles;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace Oxium.Shader
{
	public class Session : IDisposable
	{
		internal static ConcurrentDictionary<Guid, Func<string, byte[]?>> _fileFunctions = new ConcurrentDictionary<Guid, Func<string, byte[]?>>();
		
		private bool _disposedValue;
		private OXIUM_SessionHandle _handle;
		
		private Guid _guid;
		private GCHandle _guidHandle;

		internal OXIUM_SessionHandle Handle => _handle;

		public Session(OXIUM_SessionHandle handle,Guid guid,GCHandle guidHandle)
		{
			_handle = handle;
			_guid = guid;
			_guidHandle = guidHandle;
		}

		public Module? LoadModule(string moduleName,string path,string source)
		{
			unsafe
			{
				var pModuleName = Utf8StringMarshaller.ConvertToUnmanaged(moduleName);
				var pPath = Utf8StringMarshaller.ConvertToUnmanaged(path);
				var pSource = Utf8StringMarshaller.ConvertToUnmanaged(source);
				var handle = OXIUM_ShaderCompiler.oxLoadModule(Handle,pModuleName,pPath,pSource);
				if (handle.IsNull)
					return null;
				return new Module(handle);
			}
		}
		
		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		internal unsafe static byte* ReadFileCB(byte* path,nint* count,void* userData)
		{
			if (userData == null)
				return null;
			if (_fileFunctions.TryGetValue(*(Guid*)userData, out var func))
			{
				var ret = func(Utf8StringMarshaller.ConvertToManaged(path) ?? "");
				if (ret == null)
					return null;
				*count = ret.Length;
				var ptr = (byte*)NativeMemory.Alloc((nuint)ret.Length);
				Marshal.Copy(ret, 0, (nint)ptr, ret.Length);
				return ptr;
			}
			return null;
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		internal unsafe static void FreePtrCB(void* ptr,void* userData)
		{
			if(ptr != null)
				NativeMemory.Free(ptr);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_guidHandle.Free();
					_fileFunctions.TryRemove(_guid, out var _);
				}

				OXIUM_ShaderCompiler.oxDestroySession(_handle);
				_disposedValue = true;
			}
		}

		~Session()
			=> Dispose(false);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
