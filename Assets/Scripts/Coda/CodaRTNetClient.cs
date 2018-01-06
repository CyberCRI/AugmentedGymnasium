using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace CodaRTNetCSharp {

	class CodaRTNetClient
	{
		//WIN 64 DLL IMPORT
	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?InitClient@codaRTNetUnity@@SAHKGKH@Z", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool InitClient(
	        [param: MarshalAs(UnmanagedType.U4)]
	        UInt32 serverIP,
	        [param: MarshalAs(UnmanagedType.U2)]
	        UInt16 serverPort,
	        [param: MarshalAs(UnmanagedType.U4)]
	        UInt32 hardwareConfig,
	        [param: MarshalAs(UnmanagedType.Bool)]
	        bool extendedLog
	        );

	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?EndClient@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool EndClient();

	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?PrepAcq@codaRTNetUnity@@SAHKKKH@Z", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool PrepAcq(
	        [param: MarshalAs(UnmanagedType.U4)]
	        UInt32 mode,
	        [param: MarshalAs(UnmanagedType.U4)]
	        UInt32 decimation,
	        [param: MarshalAs(UnmanagedType.U4)]
	        UInt32 maxsamples,
	        [param: MarshalAs(UnmanagedType.Bool)]
	        bool extsync
	        );

	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?StartAcq@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool StartAcq();

	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?StopAcq@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool StopAcq();

	    [DllImport("codaRTNetUnity_x64", EntryPoint = "?GetData@codaRTNetUnity@@SAHKPEAMPEAE@Z", CallingConvention = CallingConvention.Cdecl)]
	    [return: MarshalAs(UnmanagedType.Bool)]
	    protected static extern bool GetData(
	        UInt32 nummarkers,
	        Single[] position,
	        byte[] valid);
        /*
         * //WIN_32 DLLIMPORT
            [DllImport("codaRTNetUnity_Win32", EntryPoint ="?InitClient@codaRTNetUnity@@SAHKGKH@Z", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool InitClient(
                [param: MarshalAs(UnmanagedType.U4)]
                UInt32 serverIP,
                [param: MarshalAs(UnmanagedType.U2)]
                UInt16 serverPort,
                [param: MarshalAs(UnmanagedType.U4)]
                UInt32 hardwareConfig,
                [param: MarshalAs(UnmanagedType.Bool)]
                bool extendedLog
                );

            [DllImport("codaRTNetUnity_Win32", EntryPoint = "?EndClient@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EndClient();

            [DllImport("codaRTNetUnity_Win32", EntryPoint = "?PrepAcq@codaRTNetUnity@@SAHKKKH@Z", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool PrepAcq(
                [param: MarshalAs(UnmanagedType.U4)]
                UInt32 mode,
                [param: MarshalAs(UnmanagedType.U4)]
                UInt32 decimation,
                [param: MarshalAs(UnmanagedType.U4)]
                UInt32 maxsamples,
                [param: MarshalAs(UnmanagedType.Bool)]
                bool extsync
                );

            [DllImport("codaRTNetUnity_Win32", EntryPoint = "?StartAcq@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool StartAcq();

            [DllImport("codaRTNetUnity_Win32", EntryPoint = "?StopAcq@codaRTNetUnity@@SAHXZ", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool StopAcq();

            [DllImport("codaRTNetUnity_Win32", EntryPoint = "?GetData@codaRTNetUnity@@SAHKPAMPAE@Z", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetData(
                UInt32 nummarkers,
                Single[] position,
                byte[] valid);
        */

        //For some reason IPAddress.Parse follows little-endian logic and so the IP address has to be reverse order. So 1.0.0.10 for an actual IPAddress 10.0.0.1
        //private static String ServerIPString = "32.0.0.10";
        private static String ServerIPString = "1.0.0.10";
        private static UInt32 ServerIPAddress;

		//This remains the same. Always. 
	    private static UInt16 ServerPort = 10111;

        //This can be found out from the Registry of the machine running RTServer
        //private static UInt32 HardwareConfigReg = 0x000003F8;

        //private static UInt32 HardwareConfigReg = 0x000003E0; //Roman Hub - Coda.1
        private static UInt32 HardwareConfigReg = 0x000003E1; //Roman Hub - Coda.1+2
        //private static UInt32 HardwareConfigReg = 0x000003F9;

        //Mode [1] : 100Hz
        //Mode [2] : 200Hz
        //Mode [3] : 400Hz
        //Mode [4] : 800Hz
        private static UInt32 CX1Mode = 1;

		//Default decimation always 1. 
	    private static UInt32 Decimation = 1;

		//MaxSamples ZERO means unlimited acquisition time. 
	    private static UInt32 MaxSamples = 0;

		//We want external sync OFF by default. 
	    private static bool ExtSync = false;

		//This will depend on CX1Mode
	    private static UInt32 MaxMarkers = 0;

		//Set this to true if you want to verify that the DLL is indeed receiving data. Lots of file writes, so careful using this. 
	    private static bool ExtendedLog = false;


		//Initializes the system with the above settings. Does StartSystem and AcqPrep consequently. In the context of Unity, call this in the Start function of MonoBehaviour.
	    public static bool InitSystem()
	    {
	        byte[] serverIPBytes = IPAddress.Parse(ServerIPString).GetAddressBytes();
	        ServerIPAddress = BitConverter.ToUInt32(serverIPBytes, 0);

	        //Close any existing connections
	        EndClient();

	        if (!InitClient(ServerIPAddress, ServerPort, HardwareConfigReg, ExtendedLog))
	            return false;

			if (!PrepAcq(CX1Mode, Decimation, MaxSamples, ExtSync))
				return false;


	        switch (CX1Mode)
	        {
	            //100 Hz - 56 Markers
	            case 1:
	                MaxMarkers = 56;
	                break;
	            //200 Hz - 28 Markers
	            case 2:
	                MaxMarkers = 28;
	                break;
	            //400 Hz - 12 Markers
	            case 3:
	                MaxMarkers = 12;
	                break;
	            //800 Hz - 6 Markers
	            case 4:
	                MaxMarkers = 6;
	                break;
	            //Unknown mode. 
	            default:
	                MaxMarkers = 0;
	                break;

	        }

	        return true;
	    }

		//Cleans up the RTNet client. Call this at the end. In the context of Unity, Call this when the script object is about to be destroyed (in MonoBehaviour OnDisable function perhaps)
	    public static bool StopSystem()
	    {
	        return EndClient();
	    }

		//Starts an (unbuffered) AcqContinuous mode acquisition. Upto the client to read the UDP buffer when needed (using GetLatestFrame)
	    public static bool StartAcquiring()
	    {

	        return StartAcq();
	    }

		//Stops an acquisition. Call this before calling StopSystem. In the context of Unity, call this in the OnDisable function. 
	    public static bool StopAcquiring()
	    {
	        return StopAcq();
	    }

		//Constructs a CodaFrame object and populates it with the latest marker XYZ and Valid array. MaxMarkers specifies the number of markers to expect. 
	    public static CodaFrame GetLatestFrame()
	    {
			Single[] markerXYZ = new Single[MaxMarkers * 3];
			byte[] markerValid = new byte[MaxMarkers];
			if (GetData (MaxMarkers, markerXYZ, markerValid)) {
				return new CodaFrame (MaxMarkers, markerXYZ, markerValid);
			}

			return null;
	    }

		//Returns the MaxMarkers variable. 
		public static UInt32 getMaxMarkers()
		{
			return MaxMarkers;
		}
	}
}
