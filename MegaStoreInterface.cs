using System;
using System.Runtime.InteropServices;
using UnityEngine;

//download megastore
//run the following command in its repo: ghc --mk-dll -o Megastore.dll A.o Super.o B.o libmine.a -lgdi32
public class MegaStoreInterface : MonoBehaviour
{
    // Importing the saveStoreFFI and loadStoreFFI functions from the Haskell library
    [DllImport("libmegastore.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern void saveStoreFFI(string path, string data);

    [DllImport("libmegastore.so", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr loadStoreFFI(string path);

    static void RuntimeTest()
    {
        // Example usage for saving a store
        string filePath = "test.megastore";
        string data     = "sampleData";
        
        saveStoreFFI(filePath, data);
        Debug.Log("Data saved!");

        // Example usage for loading a store
        IntPtr loadedDataPtr = loadStoreFFI(filePath);
        string loadedData = Marshal.PtrToStringAnsi(loadedDataPtr);
        Debug.Log("Loaded Data: " + loadedData);
    }
}