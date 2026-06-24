using System.Management;
using System.Runtime.InteropServices;

namespace Super_Burner
{
	internal static class OpticalDriveHelper
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
			out ulong lpFreeBytesAvailable,
			out ulong lpTotalNumberOfBytes,
			out ulong lpTotalNumberOfFreeBytes);

		public static List<string> GetOpticalDriveDisplayStrings()
		{
			var list = new List<string>();

			string FormatSizeFromBytes(long bytes)
			{
				double mb = bytes / (1024.0 * 1024.0);
				if (mb >= 1024) return $", {Math.Round(mb / 1024.0, 2)} GB";
				return $", {Math.Ceiling(mb)} MB";
			}

			bool TryDriveInfo(DriveInfo d, out string display)
			{
				display = d.Name.TrimEnd('\\');
				try
				{
					bool isReady = false;
					try { isReady = d.IsReady; } catch { isReady = false; }
					if (isReady)
					{
						long sizeBytes = 0;
						try { sizeBytes = d.TotalSize; } catch { sizeBytes = 0; }
						string sizeStr = sizeBytes > 0 ? FormatSizeFromBytes(sizeBytes) : string.Empty;
						if (!string.IsNullOrEmpty(d.VolumeLabel)) display = $"{display} ({d.VolumeLabel}{sizeStr})";
						else display = $"{display} ({sizeStr.TrimStart(',',' ' )})";
						return true;
					}
				}
				catch { }
				return false;
			}

			bool TryWmiLogicalDisk(string deviceId, out string display)
			{
				display = deviceId;
				try
				{
					using (var searcher = new ManagementObjectSearcher($"SELECT Size, VolumeName FROM Win32_LogicalDisk WHERE DeviceID = '{deviceId}'"))
					{
						foreach (ManagementObject mo in searcher.Get())
						{
							var sizeObj = mo["Size"];
							var volObj = mo["VolumeName"];
							long wmiSize = 0;
							if (sizeObj != null && long.TryParse(sizeObj.ToString(), out wmiSize))
							{
								string sizeStr = FormatSizeFromBytes(wmiSize);
								string vol = volObj != null ? volObj.ToString() : string.Empty;
								if (!string.IsNullOrEmpty(vol)) display = $"{deviceId} ({vol}{sizeStr})";
								else display = $"{deviceId} ({sizeStr.TrimStart(',',' ' )})";
								return true;
							}
						}
					}
				}
				catch { }
				return false;
			}

			bool TryGetDiskFreeSpaceExForDrive(string name, out string display)
			{
				display = name;
				try
				{
					string path = name + "\\";
					if (GetDiskFreeSpaceEx(path, out ulong freeAvail, out ulong totalBytes, out ulong totalFree))
					{
						if (totalBytes > 0)
						{
							display = $"{name} (Media present{FormatSizeFromBytes((long)totalBytes)})";
							return true;
						}
					}
				}
				catch { }
				return false;
			}

			bool TryWmiCdromMediaLoaded(string deviceId, out string display)
			{
				display = deviceId;
				try
				{
					using (var searcher2 = new ManagementObjectSearcher($"SELECT Drive, MediaLoaded FROM Win32_CDROMDrive WHERE Drive = '{deviceId}'"))
					{
						foreach (ManagementObject mo2 in searcher2.Get())
						{
							var mediaLoadedObj = mo2["MediaLoaded"];
							bool mediaLoaded = false;
							if (mediaLoadedObj != null && bool.TryParse(mediaLoadedObj.ToString(), out mediaLoaded) && mediaLoaded)
							{
								display = $"{deviceId} (Media present)";
								return true;
							}
						}
					}
				}
				catch { }
				return false;
			}

			long? TryImapi(string driveLetter)
			{
				try
				{
					Type recType = Type.GetTypeFromProgID("IMAPI2.MsftDiscRecorder2");
					Type fmtType = Type.GetTypeFromProgID("IMAPI2.MsftDiscFormat2Data");
					if (recType == null || fmtType == null)
						return null;

					dynamic recorder = Activator.CreateInstance(recType);
					dynamic format = Activator.CreateInstance(fmtType);

					var recObj = (object)recorder;
					try
					{
						var miInit = recObj.GetType().GetMethod("InitializeDiscRecorder") ?? recObj.GetType().GetMethod("Init") ?? recObj.GetType().GetMethod("Initialize");
						if (miInit != null)
						{
							miInit.Invoke(recObj, new object[] { driveLetter });
						}
						else
						{
							var prop = format.GetType().GetProperty("Recorder");
							if (prop != null) prop.SetValue(format, recorder);
						}
					}
					catch { }

					try { var prop = format.GetType().GetProperty("Recorder"); if (prop != null) prop.SetValue(format, recorder); } catch { }

					var fmtObj = (object)format;
					var mi = fmtObj.GetType().GetMethod("GetTotalSectorsOnMedia");
					if (mi != null)
					{
						object[] parms = new object[] { 0L };
						mi.Invoke(fmtObj, parms);
						long sectors = 0;
						try { sectors = Convert.ToInt64(parms[0]); } catch { }
						if (sectors > 0)
						{
							return sectors * 2048L;
						}
					}

					var mi2 = fmtObj.GetType().GetMethod("GetTotalBlocksOnMedia") ?? fmtObj.GetType().GetMethod("GetTotalSectorsOnMediaEx");
					if (mi2 != null)
					{
						object[] parms = new object[] { 0L };
						mi2.Invoke(fmtObj, parms);
						long blocks = 0;
						try { blocks = Convert.ToInt64(parms[0]); } catch { }
						if (blocks > 0) return blocks * 2048L;
					}

					return null;
				}
				catch { return null; }
			}

			try
			{
				var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom);
				foreach (var d in drives)
				{
					string name = d.Name.TrimEnd('\\');
					string display = name;
					string diag = string.Empty;

					if (TryDriveInfo(d, out var diDisplay))
					{
						display = diDisplay;
					}
					else if (TryWmiLogicalDisk(name, out var wmiDisplay))
					{
						display = wmiDisplay;
					}
					else if (TryGetDiskFreeSpaceExForDrive(name, out var dfDisplay))
					{
						display = dfDisplay;
					}
					else if (TryWmiCdromMediaLoaded(name, out var mediaDisplay))
					{
						display = mediaDisplay;
					}
					else
					{
						try
						{
							var imapiBytes = TryImapi(name);
							if (imapiBytes.HasValue && imapiBytes.Value > 0)
							{
								display = $"{name} ({FormatSizeFromBytes(imapiBytes.Value).TrimStart(',', ' ')})";
							}
						}
						catch { }
					}

					list.Add($"{display} {diag}");
				}
			}
			catch
			{
				// Return whatever was collected so far
			}

			return list;
		}


	}
}
