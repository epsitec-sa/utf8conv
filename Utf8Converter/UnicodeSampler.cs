//	Copyright © 2016, EPSITEC SA, CH-1400 Yverdon-les-Bains, Switzerland
//	Author: Pierre ARNAUD, Maintainer: Pierre ARNAUD

namespace Utf8Converter
{
	public static class UnicodeSampler
	{
		static UnicodeSampler()
		{
#if PLATFORM
			//	Configure Code Pages provider, so that we can resolve code page
			//	1252 using System.Text.Encoding.GetEncoding (1252).

			var provider = System.Text.CodePagesEncodingProvider.Instance;
			System.Text.Encoding.RegisterProvider (provider);
#endif
		}

		public static bool IsValidUTF8(byte[] data)
		{
			int index = 0;
			var length = data.Length;

			while (index < length)
			{
				byte c = data[index++];

				if (c <= 0x7F)
				{
					continue;
				}

				if (index >= length)
				{
					return false;
				}

				byte c1 = data[index++];

				if ((c >= 0xC2) && (c <= 0xDF))
				{
					if ((c1 >= 0x80) && (c1 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				if (index >= length)
				{
					return false;
				}

				byte c2 = data[index++];

				if (c == 0xE0)
				{
					if ((c1 >= 0xA0) && (c1 <= 0xBF) &&
						(c2 >= 0x80) && (c2 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				if ((c >= 0xE1) && (c <= 0xEF))
				{
					if ((c1 >= 0x80) && (c1 <= 0xBF) &&
						(c2 >= 0x80) && (c2 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				if (index >= length)
				{
					return false;
				}

				byte c3 = data[index++];

				if (c == 0xF0)
				{
					if ((c1 >= 0x90) && (c1 <= 0xBF) &&
						(c2 >= 0x80) && (c2 <= 0xBF) &&
						(c3 >= 0x80) && (c3 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				if ((c >= 0xF1) && (c <= 0xF3))
				{
					if ((c1 >= 0x80) && (c1 <= 0xBF) &&
						(c2 >= 0x80) && (c2 <= 0xBF) &&
						(c3 >= 0x80) && (c3 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				if (c == 0xF4)
				{
					if ((c1 >= 0x80) && (c1 <= 0x8F) &&
						(c2 >= 0x80) && (c2 <= 0xBF) &&
						(c3 >= 0x80) && (c3 <= 0xBF))
					{
						continue;
					}

					return false;
				}

				return false;
			}

			return true;
		}

		public static bool HasUtf8Bom(byte[] array)
		{
			//	This is a BOM, often added at the start of an UTF-8 file to identify it
			//	as such on the Windows platform.

			if ((array.Length >= 3) &&
				(array[0] == 0xef) &&
				(array[1] == 0xbb) &&
				(array[2] == 0xbf))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static string GetUTF8String(byte[] data)
		{
			if (UnicodeSampler.HasUtf8Bom (data))
			{
				//	Skip the BOM, keep only the UTF8 after it.
				return System.Text.Encoding.UTF8.GetString (data, 3, data.Length - 3);
			}
			else if (UnicodeSampler.IsValidUTF8 (data))
			{
				//	Looks like legitimate UTF-8, so assume it really is UTF-8:
				return System.Text.Encoding.UTF8.GetString (data, 0, data.Length);
			}
			else
			{
				//	Assume this is plain Windows ANSI Latin-1 (code page 1252)
				var encoding = System.Text.Encoding.GetEncoding (1252);
				return encoding.GetString (data, 0, data.Length);
			}
		}
	}
}
