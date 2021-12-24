using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2021
{
    internal class Day24
    {
        public static string A(string input)
        {
            ALUMultiverser ALUM = new ALUMultiverser(input);
            return ALUM.FindMax().ToString();
        }

        public static string B(string input)
        {
            ALUMultiverser ALUM = new ALUMultiverser(input);
            return ALUM.FindMin().ToString();
        }
    }

    internal class ALUMultiverser
    {
        Dictionary<ALUState, long> states;
        string[] instructions;

        public ALUMultiverser(string input)
        {
            instructions = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            states = new Dictionary<ALUState, long>();
            states.Add(new ALUState(), 0);
        }

        public long FindMax()
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                string[] parts = instructions[i].Split(' ');
                Console.WriteLine(i + " " + parts[0] + " " + states.Count);

                switch (parts[0])
                {
                    case "inp":
                        // multiverse time
                        Input(parts[1], true);
                        break;
                    default:
                        Boring(parts[0], parts[1], parts[2], true);
                        break;
                }
            }

            // find all the states where z = 0
            long max = 0;
            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                if (pair.Key.Z == 0)
                {
                    max = Math.Max(max, pair.Value);
                }
            }
            return max;
        }

        internal object FindMin()
        {
            for (int i = 0; i < instructions.Length; i++)
            {
                string[] parts = instructions[i].Split(' ');
                Console.WriteLine(i + " " + parts[0] + " " + states.Count);

                switch (parts[0])
                {
                    case "inp":
                        // multiverse time
                        Input(parts[1], false);
                        break;
                    default:
                        Boring(parts[0], parts[1], parts[2], false);
                        break;
                }
            }

            // find all the states where z = 0
            long min = long.MaxValue;
            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                if (pair.Key.Z == 0)
                {
                    min = Math.Min(min, pair.Value);
                }
            }
            return min;

        }


        private void Input(string v, bool max)
        {
            // Multiverse time!
            // If any states have a lower Z than they had at the start of the last multiverse, only keep them
            Dictionary<ALUState, long> newStates = new Dictionary<ALUState, long>();
            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                if (pair.Key.LastZ > pair.Key.Z)
                {
                    newStates.Add(pair.Key, pair.Value);
                }
            }
            if (newStates.Count > 0)
            {
                states = newStates;
            }
            // Reset all the lastValues
            newStates = new Dictionary<ALUState, long>();
            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                newStates.Add(pair.Key.Reset(), pair.Value);
            }
            states = newStates;

            // As all the registers are reset after this, merge states that have the same Z
            newStates = new Dictionary<ALUState, long>();
            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                ALUState newState = new ALUState(pair.Key.Z);
                if (newStates.ContainsKey(newState))
                {
                    if (max)
                    {
                        newStates[newState] = Math.Max(pair.Value, newStates[newState]);
                    }
                    else
                    {
                        newStates[newState] = Math.Min(pair.Value, newStates[newState]);
                    }
                }
                else
                {
                    newStates.Add(newState, pair.Value);
                }
            }
            states = newStates;
            newStates = new Dictionary<ALUState, long>();
            for (int i = 1; i <= 9; i++)
            {
                foreach (KeyValuePair<ALUState, long> pair in states)
                {
                    ALUState newState = new ALUState(pair.Key);
                    newState.SetRegister(v, i);
                    long newValue = pair.Value * 10 + i;
                    if (newStates.ContainsKey(newState))
                    {
                        if (max)
                        {
                            newStates[newState] = Math.Max(newValue, newStates[newState]);
                        }
                        else
                        {
                            newStates[newState] = Math.Min(newValue, newStates[newState]);

                        }
                    }
                    else
                    { 
                    newStates.Add(newState, newValue);
                }
                }
            }
            states = newStates;
        }

        private void Boring(string instruction, string arg1, string arg2, bool max)
        {
            Dictionary<ALUState, long> newStates = new Dictionary<ALUState, long>();

            foreach (KeyValuePair<ALUState, long> pair in states)
            {
                ALUState newState = new ALUState(pair.Key);
                newState.Process(instruction, arg1, arg2);

                // now to add to the new states dictionary
                if (newStates.ContainsKey(newState))
                {
                    if (max)
                    {
                        newStates[newState] = Math.Max(pair.Value, newStates[newState]);
                    }
                    else
                    {
                        newStates[newState] = Math.Min(pair.Value, newStates[newState]);
                    }
                }
                else
                {
                    newStates.Add(newState, pair.Value);
                }
            }

            states = newStates;
        }

    }
    internal class ALUState
    {
        long[] registers;
        private readonly static char[] registerNames = new char[] { 'w', 'x', 'y', 'z' };
        long lastZ;

        public ALUState()
        {
            registers = new long[4];            
        }

        public ALUState(ALUState orig)
        {
            registers = new long[4];
            registers[0] = orig.registers[0];
            registers[1] = orig.registers[1];
            registers[2] = orig.registers[2];
            registers[3] = orig.registers[3];
            lastZ = orig.lastZ;
        }

        public ALUState(long z)
        {
            registers = new long[4];
            registers[3] = z;   
            lastZ = z;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ALUState v = (ALUState)obj;
                return registers[0] == v.registers[0] &&
                       registers[1] == v.registers[1] &&
                       registers[2] == v.registers[2] &&
                       registers[3] == v.registers[3];
            }
        }
        public override int GetHashCode()
        {
            long hash = 0;
            foreach (long value in registers)
            {
                hash = hash * 13 + value;
            }
            return (int) (hash % int.MaxValue);
        }

        internal void Add(string v1, string v2)
        {
            SetRegister(v1, GetValue(v1) + GetValue(v2));
        }
        internal void Multiply(string v1, string v2)
        {
            SetRegister(v1, GetValue(v1) * GetValue(v2));
        }

        internal void Divide(string v1, string v2)
        {
            SetRegister(v1, GetValue(v1) / GetValue(v2));
        }


        internal void Modulo(string v1, string v2)
        {
            SetRegister(v1, GetValue(v1) % GetValue(v2));
        }
        internal void Equal(string v1, string v2)
        {
            SetRegister(v1, (GetValue(v1) == GetValue(v2))?1:0);
        }

        internal void SetRegister(string v1, long v)
        {
            if (registerNames.Contains(v1[0]))
            {
                registers[Array.IndexOf(registerNames, v1[0])] = v;
            }
            else
            {
                throw new ArgumentException("Invalid register " + v1);
            }

        }

        private long GetValue(string v1)
        {
            if (registerNames.Contains(v1[0]))
            {
                return registers[Array.IndexOf(registerNames, v1[0])];
            }
            else
            {
                return int.Parse(v1);
            }
        }

        public long W
        {
            get { return registers[0]; }
        }

        public long X
        {
            get { return registers[1]; }
        }

        public long Y
        {
            get { return registers[2]; }
        }

        public long Z
        {
            get { return registers[3]; }
        }

        public long LastZ { get => lastZ; set => lastZ = value; }

        public override string ToString()
        {
            return "{" + W + "," + X + "," + Y + "," + Z + "}";
        }

        internal void Process(string instruction, string arg1, string arg2)
        {
            switch (instruction)
            {
                case "add":
                    Add(arg1, arg2);
                    break;
                case "mul":
                    Multiply(arg1, arg2);
                    break;
                case "div":
                    Divide(arg1, arg2);
                    break;
                case "mod":
                    Modulo(arg1, arg2); 
                    break;
                case "eql":
                    Equal(arg1, arg2);
                    break;
                default:
                    throw new ArgumentException("Invalid instruction " + instruction);
            }
        }

        internal ALUState Reset()
        {
            ALUState newState = new ALUState(this);
            newState.lastZ = Z;
            return newState;
        }
    }
}
