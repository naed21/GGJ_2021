using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GGJ_2021
{
	public class SoundEffectWrapper
	{
		public string Name { get; set; }
		public float Volume { get; set; }

		public SoundEffectInstance Instance { get; set; }
		/// <summary>
		/// Just holding onto this, not sure if we need it or not after creating the instance.
		/// Worried that it'll get eaten by garbage disposal if we don't hold a ref. Doesn't instance hold a ref?
		/// </summary>
		public SoundEffect SoundEffect { get; set; }
	}
	
	public class SoundManager
	{
		public float MasterVolume { get; set; }

		Dictionary<SoundType, List<SoundEffectWrapper>> SoundEffectLibrary { get; set; }

		public SoundManager(float defaultMaster = 1f)
		{
			SoundEffectLibrary = new Dictionary<SoundType, List<SoundEffectWrapper>>();
			MasterVolume = defaultMaster;
		}

		public void AddOgg(SoundType soundType, string contentPath, string name = "")
		{
			using (MemoryStream stream = new MemoryStream())
			{
				using (var writer = new System.IO.BinaryWriter(stream))
				{
					ConvertOggToWaveStream(contentPath, writer, stream);

					stream.Position = 0;

					var soundEffect = SoundEffect.FromStream(stream);

					SoundEffectWrapper wrapper = new SoundEffectWrapper
					{
						Instance = soundEffect.CreateInstance(),
						SoundEffect = soundEffect,
						Name = name == "" ? soundEffect.Name : name,
						Volume = 1f
					};

					if (!SoundEffectLibrary.ContainsKey(soundType))
						SoundEffectLibrary.Add(soundType, new List<SoundEffectWrapper>());

					SoundEffectLibrary[soundType].Add(wrapper);
				}
			}
		}

		public void Update(TimeSpan elapsedGameTime)
		{
			/*
			 * Manage background music and effects
			 * Allow background music to transition between tracks
			 */
		}

		private static void ConvertOggToWaveStream(string oggFilePath, BinaryWriter writer, Stream output)
		{
			using (var vorbis = new NVorbis.VorbisReader(oggFilePath))
			{
				writer.Write(ASCIIEncoding.ASCII.GetBytes("RIFF"));
				writer.Write(0);
				writer.Write(ASCIIEncoding.ASCII.GetBytes("WAVE"));
				writer.Write(ASCIIEncoding.ASCII.GetBytes("fmt "));
				writer.Write(18);
				writer.Write((short)1); // PCM format
				writer.Write((short)vorbis.Channels);
				writer.Write(vorbis.SampleRate);
				writer.Write(vorbis.SampleRate * vorbis.Channels * 2);  // avg bytes per second
				writer.Write((short)(2 * vorbis.Channels)); // block align
				writer.Write((short)16); // bits per sample
				writer.Write((short)0); // extra size

				writer.Write(ASCIIEncoding.ASCII.GetBytes("data"));
				writer.Flush();
				var dataPos = output.Position;
				writer.Write(0);

				var buf = new float[vorbis.SampleRate / 10 * vorbis.Channels];
				int count;
				while ((count = vorbis.ReadSamples(buf, 0, buf.Length)) > 0)
				{
					for (int i = 0; i < count; i++)
					{
						var temp = (int)(32767f * buf[i]);
						if (temp > 32767)
						{
							temp = 32767;
						}
						else if (temp < -32768)
						{
							temp = -32768;
						}
						writer.Write((short)temp);
					}
				}
				writer.Flush();

				writer.Seek(4, System.IO.SeekOrigin.Begin);
				writer.Write((int)(output.Length - 8L));

				writer.Seek((int)dataPos, System.IO.SeekOrigin.Begin);
				writer.Write((int)(output.Length - dataPos - 4L));

				writer.Flush();
			}
		}
	}


}
