using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CodaRTNetCSharp {
	public class CodaFrame
	{
	    private UInt32 nMarkers = 0;
	    private Single[] markerPos;
	    private byte[] markerValid;

		public CodaFrame(UInt32 numMarkers, Single[] xyzArray, byte[] validArray)
	    {
	        nMarkers = numMarkers;
			markerPos = xyzArray;
			markerValid = validArray;
	    }

		public UInt32 getNumMarkers() {
			return nMarkers;
		}

	    public Vector3 getMarkerPosition(UInt32 markerID)
	    {
			if (markerID > nMarkers || markerID < 1) {
				return new Vector3();
			}

			Vector3 markerXYZ = new Vector3 ();
            int zeroBased = ((int)markerID - 1) * 3;
			markerXYZ.x = markerPos [zeroBased];
			markerXYZ.y = markerPos [zeroBased + 1];
			markerXYZ.z = markerPos [zeroBased + 2];

			return markerXYZ;
	    }

		public bool isMarkerVisible(UInt32 markerID)
		{
			if (markerID > nMarkers || markerID < 1) {
				return false;
			}

			int zeroBased = (int)markerID - 1;

			return markerValid [zeroBased] > 0 ? true : false;
		}
	}
}
