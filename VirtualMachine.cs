using System;
using System.Collections.Generic;
using System.Reflection;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }
		public Dictionary<char, Action<IVirtualMachine>> register;

        public VirtualMachine(string program, int memorySize)
		{
			Memory = new byte[memorySize];
			Instructions = program;
			register = new Dictionary<char, Action<IVirtualMachine>>();
		}

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			register.Add(symbol, (IVirtualMachine) => execute(this));
		}

        public void Run()
		{
			foreach (var action in register.Values)
			{
                action(this);
                InstructionPointer++;
            }
		}
	}
}