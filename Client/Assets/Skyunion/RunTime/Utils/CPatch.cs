using System.Runtime.InteropServices;
using UnityEngine;
using System;

public class CPatch {

    // Use this for initialization
#if (UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR
    const string cpatchlib ="__Internal";
#else
    const string cpatchlib = "cpatch";
#endif


    [DllImport(cpatchlib)]
    public static extern int happlypatch(string oldPath, string diffPath, string newPath);
    
    
     [DllImport(cpatchlib, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr desencode(IntPtr key,UInt32 key_len,IntPtr text,UInt32 text_len,ref int bufferLen);
    
    
    [DllImport(cpatchlib, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr desdecode(IntPtr key,UInt32 key_len,IntPtr text,UInt32 text_len,ref int bufferLen);

     
    [DllImport(cpatchlib, CallingConvention = CallingConvention.Cdecl)]
    private static extern void desBufferFree(IntPtr buffer);
    

    public static byte[] DesEncodeBuffer(byte[] key,byte[] source)
    {
        GCHandle kbuf = GCHandle.Alloc(key, GCHandleType.Pinned);
        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;
        ptr = desencode(kbuf.AddrOfPinnedObject(),(uint)key.Length,sbuf.AddrOfPinnedObject(), (uint)source.Length,ref siz);
        sbuf.Free();
        kbuf.Free();
        
        if (siz == 0 || ptr == IntPtr.Zero)
        {
            desBufferFree(ptr);
            return null;
        }
        
        byte[] outBuffer = new byte[siz];
        Marshal.Copy(ptr, outBuffer, 0, (int)siz);
        
        desBufferFree(ptr);
        return outBuffer;
    }
    
    public static byte[] DesDecodeBuffer(byte[] key,byte[] source,uint buffersize)
    {
        GCHandle kbuf = GCHandle.Alloc(key, GCHandleType.Pinned);
        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;
        ptr = desdecode(kbuf.AddrOfPinnedObject(),(uint)key.Length,sbuf.AddrOfPinnedObject(), buffersize,ref siz);
        kbuf.Free();
        sbuf.Free(); 
        if (siz == 0 || ptr == IntPtr.Zero)
        {
            desBufferFree(ptr);
            return null;
        }
        byte[] outBuffer = new byte[siz];
        Marshal.Copy(ptr, outBuffer, 0, (int)siz);
        desBufferFree(ptr);			
        return outBuffer;
    }
}
