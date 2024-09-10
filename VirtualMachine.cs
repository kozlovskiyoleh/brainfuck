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
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		public Dictionary<char, Action<IVirtualMachine>> Register;

        public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = TransformToInstructions(program);
			Register = new Dictionary<char, Action<IVirtualMachine>>();
		}

		private string TransformToInstructions(string program) => Regex.Replace(program, "[^0-9a-zA-Z,.<>+-\\[\\]]+", "");

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute) => Register.Add(symbol, (IVirtualMachine) => execute(this));

        public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				if (IsCommandInRegister())
				{
                    var command = Instructions[InstructionPointer];
                    Register[command](this);
				}
                InstructionPointer++;
            }
		}

		private bool IsCommandInRegister() => Register.ContainsKey(Instructions[InstructionPointer]);
	}
}