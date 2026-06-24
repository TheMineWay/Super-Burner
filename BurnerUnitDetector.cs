using System.Management;

namespace Super_Burner
{
	internal sealed class BurnerUnitDetector : IDisposable
	{
		public enum UnitAction
		{
			CONNECTED,
			DISCONNECTED
		}

		public delegate void OnUnitActionDetected(UnitAction action, string? driveName);
		public delegate void OnUnitConnectActionDetected(string? driverName);

		public static event OnUnitActionDetected? onUnitActionDetected;
		public static event OnUnitConnectActionDetected? onUnitConnected, onUnitDisconnected;
		private ManagementEventWatcher? _watcher;

		public void Start()
		{
			if (_watcher is not null)
				return;

			/*
             * EventType:
             * 2 = arrival / volume connected
             * 3 = exit / volume disconnected
             */
			const string query = """
                SELECT * FROM Win32_VolumeChangeEvent
                WHERE EventType = 2 OR EventType = 3
                """;

			_watcher = new ManagementEventWatcher(query);
			_watcher.EventArrived += OnEventArrived;
			_watcher.Start();
		}

		public void Stop()
		{
			if (_watcher is null)
				return;

			_watcher.EventArrived -= OnEventArrived;
			_watcher.Stop();
			_watcher.Dispose();
			_watcher = null;
		}

		private void OnEventArrived(object sender, EventArrivedEventArgs e)
		{
			ushort eventType = Convert.ToUInt16(e.NewEvent["EventType"]);
			string? driveName = e.NewEvent["DriveName"]?.ToString();

			if (string.IsNullOrWhiteSpace(driveName))
				return;

			bool isCdOrDvdUnit = IsCdOrDvdDrive(driveName);

			if (!isCdOrDvdUnit)
				return;

			UnitAction action = eventType switch
			{
				2 => UnitAction.CONNECTED,
				3 => UnitAction.DISCONNECTED,
				_ => throw new InvalidOperationException($"Not supported event: {eventType}")
			};

			onUnitActionDetected?.Invoke(action, driveName);

			OnUnitConnectActionDetected? connectedAction = onUnitConnected;
			if (action == UnitAction.DISCONNECTED) connectedAction = onUnitDisconnected;
			connectedAction?.Invoke(driveName);
		}

		private static bool IsCdOrDvdDrive(string driveName)
		{
			try
			{
				string root = driveName.EndsWith('\\')
					? driveName
					: driveName + "\\";

				DriveInfo driveInfo = new(root);

				return driveInfo.DriveType == DriveType.CDRom;
			}
			catch
			{
				return false;
			}
		}

		public void Dispose()
		{
			Stop();
		}
	}
}