using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day16
    {
        public static string A(string input)
        {
            string bitString = "";
            foreach (char c in input)
            {
                bitString += Packet.HexToBitString(c);
            }
            Packet.CreatePacket(bitString, out Packet packet);
            return packet.VersionSum().ToString();
        }

        public static string B(string input)
        {
            string bitString = "";
            foreach (char c in input)
            {
                bitString += Packet.HexToBitString(c);
            }
            Packet.CreatePacket(bitString, out Packet packet);
            return packet.GetValue().ToString();
        }

    }

    internal class Packet
    {
        private short version;
        private short typeId;
        private List<Packet> innerPackets;
        private long value;

        public short Version { get => version; set => version = value; }
        public short TypeId { get => typeId; set => typeId = value; }
        public long Value { set => this.value = value; }

        public static string CreatePacket(string input, out Packet packet)
        {
            packet = new Packet();

            // first 3 bits are the version
            packet.Version = (short)ConvertBitStringToLong(input.Substring(0, 3));

            // next 3 bits are type id
            packet.TypeId = (short)ConvertBitStringToLong(input.Substring(3, 3));

            if (packet.TypeId == 4)
            {
                // We've a literal value packet
                input = input.Substring(6);
                string numberString = "";
                bool moreNumber = true;
                while (moreNumber)
                {
                    moreNumber = input[0] == '1';
                    // next 4 digits are part of the number
                    numberString += input.Substring(1, 4);
                    input = input.Substring(5);
                }

                packet.Value = ConvertBitStringToLong(numberString);
            }
            else
            {
                // We've an operator packet
                input = input.Substring(6);
                if (input[0] == '0')
                {
                    // total length in bits
                    // the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
                    long innerLength = ConvertBitStringToLong(input.Substring(1, 15));
                    string innerString = input.Substring(16, (int)innerLength);
                    input = input.Substring(16 + (int)innerLength);
                    packet.AddSubPackets(CreatePackets(innerString));
                }
                else
                {
                    // number of sub-packets immediately contained
                    // the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
                    long innerCount = ConvertBitStringToLong(input.Substring(1, 11));
                    input = input.Substring(12);
                    while (innerCount > 0)
                    {
                        Packet newPacket;
                        input = CreatePacket(input, out newPacket);
                        packet.AddSubPacket(newPacket);
                        innerCount--;
                    }
                }
            }

            return input;
        }


        private static List<Packet> CreatePackets(string input)
        {
            // keep making packets till the string doesn't exist any more
            List<Packet> packets = new List<Packet>();
            while (!DeadString(input))
            {
                Packet newPacket;
                input = CreatePacket(input, out newPacket);
                packets.Add(newPacket);
            }
            return packets;
        }
        private void AddSubPacket(Packet newPacket)
        {
            innerPackets.Add(newPacket);
        }

        private void AddSubPackets(List<Packet> packets)
        {
            innerPackets.AddRange(packets);
        }


        public Packet()
        {
            innerPackets = new List<Packet>();
        }

        private static long ConvertBitStringToLong(string bitString)
        {
            long value = 0;
            for (int i = 0; i < bitString.Length; i++)
            {
                value *= 2;
                value += bitString[i] - '0';
            }
            return value;
        }

        private static bool DeadString(string input)
        {
            return input.All(x => x == 0);
        }

        internal long VersionSum()
        {
            long sum = version;
            foreach (Packet packet in innerPackets)
            {
                sum += packet.VersionSum();
            }
            return sum;
        }

        internal long GetValue()
        {
            switch (typeId)
            {
                case 0:
                    // sum
                    return innerPackets.Sum(x => x.GetValue());
                case 1:
                    // product
                    return innerPackets.Aggregate((long)1, (total, packet) => total * packet.GetValue());
                case 2:
                    // min
                    return innerPackets.Min(x => x.GetValue());
                case 3:
                    return innerPackets.Max(x => x.GetValue());
                case 5:
                    // greater than
                    return innerPackets[0].GetValue() > innerPackets[1].GetValue() ? 1 : 0;
                case 6:
                    // less than
                    return innerPackets[0].GetValue() < innerPackets[1].GetValue() ? 1 : 0;
                case 7:
                    // equal to
                    return innerPackets[0].GetValue() == innerPackets[1].GetValue() ? 1 : 0;
                default:
                    return value;
            }
        }

        internal static string HexToBitString(char c)
        {
            string output = "";
            short num = (short)(c - '0');
            if (num > 9)
            {
                num = (short)(c - 'A' + 10);
            }

            for (int i = 0; i < 4; i++)
            {
                if (num %2 == 1)
                {
                    output = '1' + output;
                }
                else

                {
                    output = '0' + output;
                }
                num /= 2;
            }

            return output;
        }
    }
}

