using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;

namespace BEEP
{
    class Program
    {
        static int[] FREQ = { 25, 28, 31, 33, 37, 41, 44, 49, 55, 62, 65, 73, 82, 87, 98, 110, 123, 131, 147, 165, 175, 196, 220, 247, 262, 294, 330, 349, 392, 440, 494 };
        static string[] freq = { "G0", "A0", "H0", "C1", "D1", "E1", "F1", "G1", "A1", "H1", "C2", "D2", "E2", "F2", "G2", "A2", "H2", "C3", "D3", "E3", "F3", "G3", "A3", "H3", "C4", "D4", "E4", "F4", "G4", "A4", "H4" };
        static void Main(string[] args)
        {
            string song = ",F44,H38,C48,D44,C48,H38,A34,A38,C48,E44,D48,C48,H34,C48,D44,E44,C44,A34,A32,D44,F48,A44,G48,F48,E44,C48,E44,D48,C48,H34,H38,C48,D44,E44,C44,A34,A32,D44,F48,A44,G48,F48,E44,C48,E48,F416,E416,D48,C48,H34,C48,D44,E44,C44,A34,A32,E42,C42,D42,H32,C42,A32,A32,H32,E42,C42,D42,H32,C44,E44,A44,A44,A42,";
            int note2 =-1;
            int note1;

            for (int i = 0; i < i + 1; i++)
            {
                for (int ii = 0; ii < 73; ii++)
                {
                    note1 = song.IndexOf(",", note2 + 1) + 1;
                    note2 = song.IndexOf(",", note1 + 1) - 2;
                    int noteL = Int32.Parse(song.Substring(note1 + 2, note2 - note1));
                    int noteO = Int32.Parse(song.Substring(note1 + 1, 1));
                    string noteV = song.Substring(note1, 1);
                    int indx = Array.IndexOf(freq, noteV + noteO);
                    //Console.Out.WriteLine("\nNuta: " + note + "\nIndex:"+ indx + " ("+FREQ[indx]+")" + "\nSpeed" + noteL);
                    Console.Out.Write("Nuta: " + noteV + " Oktawa: " + noteO + " Dlugosc: 1/" + noteL + " " + ii + "\n");
                    BeepBeep(1000, FREQ[indx], 857 * 1 / noteL);
                }
                note2 = -1;
                note1 = 0;
            }
        }
        private static void BeepBeep(double Amplitude, double Frequency, double Duration)
        {
            Duration += Duration * 0.1;

            double Amp = ((Amplitude * (System.Math.Pow(2, 15))) / 1000) - 1;
            double DeltaFT = 2 * Math.PI * Frequency / 44100.0;

            int Samples = (int)(441.0 * Duration / 10.0);
            int Bytes = Samples * sizeof(int);
            int[] Hdr = { 0X46464952, 36 + Bytes, 0X45564157, 0X20746D66, 16, 0X20001, 44100, 176400, 0X100004, 0X61746164, Bytes };

            using (MemoryStream MS = new MemoryStream(44 + Bytes))
            {
                using (BinaryWriter BW = new BinaryWriter(MS))
                {
                    for (int I = 0; I < Hdr.Length; I++)
                    {
                        BW.Write(Hdr[I]);
                    }
                    for (int T = 0; T < Samples; T++)
                    {
                        short Sample = System.Convert.ToInt16(Amp * Math.Sin(DeltaFT * T));
                        BW.Write(Sample);
                        BW.Write(Sample);
                    }

                    BW.Flush();
                    MS.Seek(0, SeekOrigin.Begin);
                    using (SoundPlayer SP = new SoundPlayer(MS))
                    {
                        SP.PlaySync();
                    }
                }
            }

            System.Threading.Thread.Sleep(20);
        }
    }
}

