using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; } // напевно використовується для перетворення + в символ
		public int MemoryPointer { get; set; }
		public Dictionary<char, Action<IVirtualMachine>> Register;

        public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = TransformToInstructions(program);
			Register = new Dictionary<char, Action<IVirtualMachine>>();
			//Memory = TransformInstructionsToByte(program, memorySize);
		}

		private string TransformToInstructions(string program) => Regex.Replace(program, "[^0-9a-zA-Z,.<>+-]+", "");
	
        private byte[] TransformInstructionsToByte(string program, int memorySize)
        {
            byte[] tempMemory = new byte[memorySize];
            for (int i = 0; i < program.Length; i++)
			{
				tempMemory[i] = Convert.ToByte(program[i]);
            }
            return tempMemory;
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			Register.Add(symbol, (IVirtualMachine) => execute(this));
		}

        public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				if (IsCommandInRegister())
				{
                    var command = Instructions[InstructionPointer];
                    Register[command](this);
				}
				else
				{
                    MemoryPointer++;
                    Memory[MemoryPointer] = Convert.ToByte(Instructions[InstructionPointer]);
                }
                InstructionPointer++;
            }
		}

		private bool IsCommandInRegister() => Register.ContainsKey(Instructions[InstructionPointer]);
	}
}