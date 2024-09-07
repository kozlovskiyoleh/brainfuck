using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public delegate void RegisterDelegate(char start, char end, IVirtualMachine vm);

		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => { write.Invoke(Convert.ToChar(b.Memory[b.MemoryPointer]));});
			vm.RegisterCommand('+', b => 
			{
				if (vm.Memory[vm.MemoryPointer] > Byte.MaxValue-1)
					vm.Memory[vm.MemoryPointer] = 0;
				else
                    vm.Memory[vm.MemoryPointer]++;
            });

			vm.RegisterCommand('-', b =>
            {
                if (vm.Memory[vm.MemoryPointer] <= byte.MinValue)
                    vm.Memory[vm.MemoryPointer] = Byte.MaxValue;
                else
                    vm.Memory[vm.MemoryPointer]--;
            });

            vm.RegisterCommand(',', b => { vm.Memory[b.MemoryPointer] = Convert.ToByte(read.Invoke()); });
			vm.RegisterCommand('>', b => 
			{
				if (vm.MemoryPointer >= vm.Memory.Length-1)
					vm.MemoryPointer = 0;
				else
					vm.MemoryPointer++; 
			});

			vm.RegisterCommand('<', b => 
			{ 
				if(vm.MemoryPointer <= 0)
					vm.MemoryPointer = vm.Memory.Length - 1;
				else
					vm.MemoryPointer--;
			});

			RegisterDelegate registerSymbols = RegisterLettersAndNumbers;
			registerSymbols('0', '9', vm);
			registerSymbols('A', 'Z', vm);
			registerSymbols('a', 'z', vm);
		}
		public static void RegisterLettersAndNumbers(char startSymbol, char endSymbol, IVirtualMachine vm)
		{
			for (char i = startSymbol; i <= endSymbol; i++)
			{
				char tempChar = i;
                vm.RegisterCommand(i, b => { vm.Memory[vm.MemoryPointer] = Convert.ToByte(tempChar); });
            }
		}
	}
}