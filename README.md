# MEGASTORE-CSHARP
This is a C# interface and a build of megastore that can be used with ghc to create a .dll. This is intended for use with unity and effectively constitutes a Foreign Function Interface (FFI). Untested and not intended for public use


## Creating a DLL w/ ghc (from [GHC Manual](https://downloads.haskell.org/~ghc/4.06/docs/users_guide/win32-dlls-create.html))
Sealing up your Haskell library inside a DLL is quite straightforward; compile up the object files that make up the library, and then build the DLL by issuing the following command:

```sh$ ghc --mk-dll -o HSsuper.dll A.o Super.o B.o libmine.a -lgdi32```

By feeding the ghc compiler driver the option --mk-dll, it will build a DLL rather than produce an executable. The DLL will consist of all the object files and archives given on the command line.

A couple of things to notice:


When compiling the module A, the code emitted by the compiler differs depending on whether or not the functions and data it is importing from other Haskell modules correspond to symbols that are packaged up in a ghc-compiled DLL. To resolve whether such imports are 'DLL imports' or not, the following rules are used:

If the compiler imports from a module that's in the same directory as the one being compiled, it is assumed to not belong to a different DLL (or executable) than the module being processed, so none of the same-directory imports are considered 'DLL imports'.

If a directory contains the (probably empty) file dLL_ifs.hi, the code corresponding to the interface files found in that directory are assumed to live in a DLL separate from the one being compiled. Notice that the first rule takes precedence over this one, so if you're compiling a module that imports from a Haskell module whose interface file live in the same directory, and that directory also contains the file dLL_ifs.hi, the import is still not being considered to be a 'DLL import'.

If compiling with the option -static, the previous rule is disabled.

So, in short, after having built your Haskell DLL, make sure you create the file dLL_ifs.hi in the directory that contains its interface files. If you don't, Haskell code that calls upon entry points in that DLL, will do so incorrectly, and a crash will result. (it is unfortunate that this isn't currently caught at compile-time).

By default, the entry points of all the object files will be exported from the DLL when using --mk-dll. Should you want to constrain this, you can specify the module definition file to use on the command line as follows:
```sh$ ghc --mk-dll -o .... -optdll--def -optdllMyDef.def```

See Microsoft documentation for details, but a module definition file simply lists what entry points you want to export. Here's one that's suitable when building a Haskell COM server DLL:
```
EXPORTS
 DllCanUnloadNow     = DllCanUnloadNow@0
 DllGetClassObject   = DllGetClassObject@12
 DllRegisterServer   = DllRegisterServer@0
 DllUnregisterServer = DllUnregisterServer@0
```
In addition to creating a DLL, the --mk-dll option will also create an import library. The import library name is derived from the name of the DLL, as follows:

```DLL: HScool.dll  ==> import lib: libHScool_imp.a```
The naming scheme may look a bit weird, but it has the purpose of allowing the co-existence of import libraries with ordinary static libraries (e.g., libHSfoo.a and libHSfoo_imp.a. Additionally, when the compiler driver is linking in non-static mode, it will rewrite occurrence of -lHSfoo on the command line to -lHSfoo_imp. By doing this for you, switching from non-static to static linking is simply a question of adding -static to your command line.


## Connecting the .dll to Unity 
Your application code is compiled by Unity itself, not by Visual Studio or MonoDevelop, therefore you need to make sure that Unity is aware of any external DLL libraries. To use an external DLL in your game, just place it inside of your Unity Project, in the Assets directory. Then, next time Unity synchronizes Visual Studio/MonoDevelop project, it’ll add the necessary references to the DLLs in your Visual Studio/MonoDevelop project.The external DLLs you add to your Unity project are compiled using the same process as the rest of your application code. If you encounter any difficulties including DLLs, make sure that you review the Script Compilation documentation.

Thus drop the built .dll/.so into Unity with the .cs interface then try accessing it, enjoy and good luck!
