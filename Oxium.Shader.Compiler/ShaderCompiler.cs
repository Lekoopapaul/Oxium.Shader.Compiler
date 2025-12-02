using Oxium.Shader.Unsafe;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.InteropServices.Marshalling;
using Oxium.Shader.Unsafe.Handles;

namespace Oxium.Shader
{
	public class ShaderCompiler: IDisposable
	{
		private static ConcurrentDictionary<Guid,Action<string>> _logFunctions = new ConcurrentDictionary<Guid, Action<string>>();
		private Guid _guid;
		private GCHandle _guidHandle;
		private bool _disposedValue;
		private OXIUM_ShaderCompilerHandle _handle;

		internal OXIUM_ShaderCompilerHandle Handle => _handle;

		private ShaderCompiler(OXIUM_ShaderCompilerHandle handle,Guid guid,GCHandle guidHandle)
		{
			_handle = handle;
			_guid = guid;
			_guidHandle = guidHandle;
		}

		public static ShaderCompiler? Create(Action<string> logFunction)
		{
			var guid = Guid.NewGuid();
			_logFunctions.TryAdd(guid, logFunction);
			var guidHandle = GCHandle.Alloc(guid, GCHandleType.Pinned);
			unsafe
			{
				var handle = OXIUM_ShaderCompiler.oxCreateShaderCompiler(&LogMessage, (void*)guidHandle.AddrOfPinnedObject());
				if (handle.IsNull)
					return null;
				return new ShaderCompiler(handle, guid, guidHandle);
			}
		}

		public Session? CreateSession(string profile, int optimizationLevel, Func<string, byte[]?> readFile, ReadOnlySpan<string> searchPaths, Dictionary<string, string> macros)
		{
			OXIUM_SessionHandle handle = OXIUM_SessionHandle.Null;
			var guid = Guid.NewGuid();
			var guidHandle = GCHandle.Alloc(guid, GCHandleType.Pinned);
			Session._fileFunctions.TryAdd(guid, readFile);
			unsafe
			{
				SessionDesc desc = new();
				desc.loadFile = &Session.ReadFileCB;
				desc.freePtr = &Session.FreePtrCB;
				desc.profile = AnsiStringMarshaller.ConvertToUnmanaged(profile);
				desc.OptimizationLevel = optimizationLevel;
				desc.userData = (void*)guidHandle.AddrOfPinnedObject();

				byte*[] pSearchPaths = new byte*[searchPaths.Length];
				for (int i = 0; i < searchPaths.Length; i++)
					pSearchPaths[i] = Utf8StringMarshaller.ConvertToUnmanaged(searchPaths[i]);

				Defines[] defines = new Defines[macros.Count];
				for (int i = 0; i < macros.Count; i++)
				{
					var array = macros.ToArray();
					defines[i] = new Defines() { name = Utf8StringMarshaller.ConvertToUnmanaged(array[i].Key), value = Utf8StringMarshaller.ConvertToUnmanaged(array[i].Value) };
				}

				fixed (Defines* pDefines = defines)
				fixed (byte** ppSearchPaths = pSearchPaths)
				{
					desc.searchPaths = (byte**)ppSearchPaths;
					desc.searchPathCount = pSearchPaths.Length;
					desc.macros = pDefines;
					desc.macroCount = defines.Length;

					handle = OXIUM_ShaderCompiler.oxCreateSession(Handle, &desc);
				}

				foreach (var define in defines)
				{
					Utf8StringMarshaller.Free(define.name);
					Utf8StringMarshaller.Free(define.value);
				}
				foreach (var searchPath in pSearchPaths)
					Utf8StringMarshaller.Free(searchPath);

				AnsiStringMarshaller.Free(desc.profile);
				if (handle.IsNull)
					return null;
				return new Session(handle,guid,guidHandle);
			}
		}

		[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
		private unsafe static void LogMessage(byte* msg,void* userData)
		{
			if (userData == null)
				return;
			if(_logFunctions.TryGetValue(*(Guid*)userData,out var func))
				func(Utf8StringMarshaller.ConvertToManaged(msg) ?? "Error while converting to managed string");
		}


		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_guidHandle.Free();
					_logFunctions.TryRemove(_guid, out var _);
				}

				OXIUM_ShaderCompiler.oxDestroyShaderCompiler(_handle);
				_disposedValue = true;
			}
		}

		~ShaderCompiler()
			=> Dispose(false);

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
