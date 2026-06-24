namespace Super_Burner
{
	internal static class FileEssentials
	{
		public static void InitializateEssentials() {
			if (!Directory.Exists(Constants.BURN_DIR))
			{
				Directory.CreateDirectory(Constants.BURN_DIR);
			}

			if (!File.Exists(Constants.BURN_INDEX_FILE))
			{
				File.WriteAllText(Constants.BURN_INDEX_FILE, "{}");
			}
		}

		public static void DeleteAllFiles() {
			// Do not clean burn dir
			File.Delete(Constants.BURN_INDEX_FILE);
		}

		public static void ResetFiles() {
			DeleteAllFiles();
			InitializateEssentials();
		}
	}
}
