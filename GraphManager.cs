using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using DirectShowLib;

namespace RoxioGameCap {
    class GraphManager {
        private FilterGraph graph;

        public GraphManager() {
            this.graph = new FilterGraph();
            this.getVideoWindow().put_Visible(OABool.False);
        }

        ~GraphManager() {
            this.stop();
            this.killOutput();
        }

        public void buildFromFile(string fileName) {
            int hr = 0;
            IStorage storage = null;
            IStream stream = null;
            IGraphBuilder graphBuilder = this.graph as IGraphBuilder;

            try {
                if (NativeMethods.StgIsStorageFile(fileName) != 0)
                    throw new ArgumentException();

                hr = NativeMethods.StgOpenStorage(
                    fileName,
                    null,
                    STGM.Transacted | STGM.Read | STGM.ShareDenyWrite,
                    IntPtr.Zero,
                    0,
                    out storage
                    );

                Marshal.ThrowExceptionForHR(hr);

                hr = storage.OpenStream(
                    @"ActiveMovieGraph",
                    IntPtr.Zero,
                    STGM.Read | STGM.ShareExclusive,
                    0,
                    out stream
                    );

                Marshal.ThrowExceptionForHR(hr);

                hr = (graphBuilder as IPersistStream).Load(stream);
                Marshal.ThrowExceptionForHR(hr);
            } finally {
                if (stream != null)
                    Marshal.ReleaseComObject(stream);
                if (storage != null)
                    Marshal.ReleaseComObject(storage);
            }
        }

        public void stopFileWriter() {
            int hr = 0;
            IBaseFilter fileWriter = this.findFilter("File Writer");

            this.stop();

            hr = (this.graph as IFilterGraph).RemoveFilter(fileWriter);
            checkHR(hr, "Couldn't remove file writer");

            this.run();
        }

        public void startFileWriter(string fileName) {
            int hr = 0;
            Guid fileWriterGuid = new Guid("B858031F-C578-4FDA-B8FE-7444EFD81A65");
            IBaseFilter fileWriter = Activator.CreateInstance(Type.GetTypeFromCLSID(fileWriterGuid)) as IBaseFilter;
            IBaseFilter tee = this.findFilter("Infinite Pin Tee Filter");

            this.stop();

            hr = (this.graph as IFilterGraph).AddFilter(fileWriter, "File Writer");
            checkHR(hr, "Couldn't add file writer to graph");

            hr = (fileWriter as IFileSinkFilter).SetFileName(fileName, null);
            checkHR(hr, "Couldn't set filename on file writer");

            hr = (this.graph as IFilterGraph).ConnectDirect(this.findPin(tee, "Output2"), this.findPin(fileWriter, "Input"), null);
            checkHR(hr, "Couldn't connect tee output to file writer input");

            this.run();
        }

        private IVideoWindow getVideoWindow() {
            return this.graph as IVideoWindow;
        }

        public void constrainOutputToPanel(System.Windows.Forms.Panel panel) {
            IVideoWindow videoWindow = this.getVideoWindow();

            videoWindow.put_Owner(panel.Handle);
            videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipSiblings);
            videoWindow.SetWindowPosition(
                panel.ClientRectangle.Left,
                panel.ClientRectangle.Top,
                panel.ClientRectangle.Width,
                panel.ClientRectangle.Height
            );
            videoWindow.put_MessageDrain(panel.Handle);
            videoWindow.put_Visible(OABool.True);
        }

        private void killOutput() {
            IVideoWindow videoWindow = this.getVideoWindow();
            videoWindow.put_Visible(OABool.False);
            videoWindow.put_Owner(IntPtr.Zero);
        }

        private IMediaControl getMediaControl() {
            return this.graph as IMediaControl;
        }

        private IBaseFilter findFilter(string name) {
            int hr = 0;

            IEnumFilters eFilters = null;
            hr = (this.graph as IGraphBuilder).EnumFilters(out eFilters);
            checkHR(hr, "Couldn't enumerate filters in graph");

            IntPtr pFiltersFetched = Marshal.AllocCoTaskMem(4);
            IBaseFilter[] pFilter = new IBaseFilter[1];

            while (eFilters.Next(1, pFilter, pFiltersFetched) == 0) {
                FilterInfo pInfo;
                pFilter[0].QueryFilterInfo(out pInfo);
                bool found = (pInfo.achName == name);

                if (found) {
                    return pFilter[0];
                }
            }

            return null;
        }

        private IPin findPin(IBaseFilter filter, string pinName) {
            IEnumPins ePins;
            int hr = 0;

            hr = filter.EnumPins(out ePins);
            checkHR(hr, "Couldn't enumerate pins on filter");
            IntPtr fetched = Marshal.AllocCoTaskMem(4);
            IPin[] pins = new IPin[1];
            while (ePins.Next(1, pins, fetched) == 0) {
                PinInfo pinInfo;
                pins[0].QueryPinInfo(out pinInfo);
                bool found = (pinInfo.name == pinName);
                DsUtils.FreePinInfo(pinInfo);

                if (found) {
                    return pins[0];
                }
            }

            checkHR(-1, "Couldn't find pin `" + pinName + "`");
            return null;
        }

        public void run() {
            int hr = 0;
            FilterState filterStates;

            hr = this.getMediaControl().GetState(1000, out filterStates);
            checkHR(hr, "Couldn't get graph state");

            hr = this.getMediaControl().Run();
            checkHR(hr, "Couldn't run graph");
        }

        public void stop() {
            int hr = 0;
            hr = this.getMediaControl().Stop();
            checkHR(hr, "Couldn't stop graph");
        }

        private static void checkHR(int hr, string msg) {
            if (hr < 0) {
                Console.WriteLine("Error: " + msg);
                DsError.ThrowExceptionForHR(hr);
            }
        }
    }

    #region Unmanaged Code declarations

    [Flags]
    internal enum STGM {
        Read = 0x00000000,
        Write = 0x00000001,
        ReadWrite = 0x00000002,
        ShareDenyNone = 0x00000040,
        ShareDenyRead = 0x00000030,
        ShareDenyWrite = 0x00000020,
        ShareExclusive = 0x00000010,
        Priority = 0x00040000,
        Create = 0x00001000,
        Convert = 0x00020000,
        FailIfThere = 0x00000000,
        Direct = 0x00000000,
        Transacted = 0x00010000,
        NoScratch = 0x00100000,
        NoSnapShot = 0x00200000,
        Simple = 0x08000000,
        DirectSWMR = 0x00400000,
        DeleteOnRelease = 0x04000000,
    }

    [Flags]
    internal enum STGC {
        Default = 0,
        Overwrite = 1,
        OnlyIfCurrent = 2,
        DangerouslyCommitMerelyToDiskCache = 4,
        Consolidate = 8
    }

    [Guid("0000000b-0000-0000-C000-000000000046"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IStorage {
        [PreserveSig]
        int CreateStream(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved1,
            [In] int reserved2,
#if USING_NET11
			[Out] out UCOMIStream ppstm
#else
 [Out] out IStream ppstm
#endif
);

        [PreserveSig]
        int OpenStream(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IntPtr reserved1,
            [In] STGM grfMode,
            [In] int reserved2,
#if USING_NET11
			[Out] out UCOMIStream ppstm
#else
 [Out] out IStream ppstm
#endif
);

        [PreserveSig]
        int CreateStorage(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved1,
            [In] int reserved2,
            [Out] out IStorage ppstg
            );

        [PreserveSig]
        int OpenStorage(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgPriority,
            [In] STGM grfMode,
            [In] int snbExclude,
            [In] int reserved,
            [Out] out IStorage ppstg
            );

        [PreserveSig]
        int CopyTo(
            [In] int ciidExclude,
            [In] Guid[] rgiidExclude,
            [In] string[] snbExclude,
            [In] IStorage pstgDest
            );

        [PreserveSig]
        int MoveElementTo(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgDest,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName,
            [In] STGM grfFlags
            );

        [PreserveSig]
        int Commit([In] STGC grfCommitFlags);

        [PreserveSig]
        int Revert();

        [PreserveSig]
        int EnumElements(
            [In] int reserved1,
            [In] IntPtr reserved2,
            [In] int reserved3,
            [Out, MarshalAs(UnmanagedType.Interface)] out object ppenum
            );

        [PreserveSig]
        int DestroyElement([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

        [PreserveSig]
        int RenameElement(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsOldName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsNewName
            );

        [PreserveSig]
        int SetElementTimes(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME pctime,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME patime,
            [In] System.Runtime.InteropServices.ComTypes.FILETIME pmtime
         );

        [PreserveSig]
        int SetClass([In, MarshalAs(UnmanagedType.LPStruct)] Guid clsid);

        [PreserveSig]
        int SetStateBits(
            [In] int grfStateBits,
            [In] int grfMask
            );

        [PreserveSig]
        int Stat(
#if USING_NET11
			[Out] out STATSTG pStatStg, 
#else
[Out] out System.Runtime.InteropServices.ComTypes.STATSTG pStatStg,
#endif
 [In] int grfStatFlag
 );
    }

    internal sealed class NativeMethods {
        private NativeMethods() { }

        [DllImport("ole32.dll")]
#if USING_NET11
		public static extern int CreateBindCtx(int reserved, out UCOMIBindCtx ppbc);
#else
        public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);
#endif

        [DllImport("ole32.dll")]
#if USING_NET11
		public static extern int MkParseDisplayName(UCOMIBindCtx pcb, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out int pchEaten, out UCOMIMoniker ppmk);
#else
        public static extern int MkParseDisplayName(IBindCtx pcb, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out int pchEaten, out IMoniker ppmk);
#endif

        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int OleCreatePropertyFrame(
            [In] IntPtr hwndOwner,
            [In] int x,
            [In] int y,
            [In, MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            [In] int cObjects,
            [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] object[] ppUnk,
            [In] int cPages,
            [In] IntPtr pPageClsID,
            [In] int lcid,
            [In] int dwReserved,
            [In] IntPtr pvReserved
            );

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgCreateDocfile(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] STGM grfMode,
            [In] int reserved,
            [Out] out IStorage ppstgOpen
            );

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgIsStorageFile([In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName);

        [DllImport("ole32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int StgOpenStorage(
            [In, MarshalAs(UnmanagedType.LPWStr)] string pwcsName,
            [In] IStorage pstgPriority,
            [In] STGM grfMode,
            [In] IntPtr snbExclude,
            [In] int reserved,
            [Out] out IStorage ppstgOpen
            );

    }
    #endregion
}
