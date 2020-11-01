// This file is provided under The MIT License as part of Steamworks.NET.
// Copyright (c) 2013-2019 Riley Labrecque
// Please see the included LICENSE.txt for additional information.

// This file is automatically generated.
// Changes to this file will be reverted when you update Steamworks.NET

#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
	#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS

using System.Runtime.InteropServices;
using IntPtr = System.IntPtr;

namespace Steamworks {
	[System.Serializable]
	public struct RemotePlaySessionID_t : System.IEquatable<RemotePlaySessionID_t>, System.IComparable<RemotePlaySessionID_t> {
		public uint m_RemotePlaySessionID;

		public RemotePlaySessionID_t(uint value) {
			m_RemotePlaySessionID = value;
		}

		public override string ToString() {
			return m_RemotePlaySessionID.ToString();
		}

		public override bool Equals(object other) {
			return other is RemotePlaySessionID_t && this == (RemotePlaySessionID_t)other;
		}

		public override int GetHashCode() {
			return m_RemotePlaySessionID.GetHashCode();
		}

		public static bool operator ==(RemotePlaySessionID_t x, RemotePlaySessionID_t y) {
			return x.m_RemotePlaySessionID == y.m_RemotePlaySessionID;
		}

		public static bool operator !=(RemotePlaySessionID_t x, RemotePlaySessionID_t y) {
			return !(x == y);
		}

		public static explicit operator RemotePlaySessionID_t(uint value) {
			return new RemotePlaySessionID_t(value);
		}

		public static explicit operator uint(RemotePlaySessionID_t that) {
			return that.m_RemotePlaySessionID;
		}

		public bool Equals(RemotePlaySessionID_t other) {
			return m_RemotePlaySessionID == other.m_RemotePlaySessionID;
		}

		public int CompareTo(RemotePlaySessionID_t other) {
			return m_RemotePlaySessionID.CompareTo(other.m_RemotePlaySessionID);
		}
	}
}

#endif // !DISABLESTEAMWORKS
