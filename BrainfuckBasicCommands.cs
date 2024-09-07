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
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			RegisterWriteLine(vm, write);
			RegisterReadLine(vm, read);
			RegisterIncrement(vm);
			RegisterDecrement(vm);
			RegisterNext(vm);
            RegisterPrevious(vm);
            RegisterLettersAndNumbers('0', '9', vm);
            RegisterLettersAndNumbers('A', 'Z', vm);
			RegisterLettersAndNumbers('a', 'z', vm);
		}

		private static void RegisterWriteLine(IVirtualMachine vm, Action<char> write)
		{
            vm.RegisterCommand('.', b => { write.Invoke(Convert.ToChar(b.Memory[b.MemoryPointer])); });
        }
	
		private static void RegisterReadLine(IVirtualMachine vm, Func<int> read)
		{
            vm.RegisterCommand(',', b => { vm.Memory[b.MemoryPointer] = Convert.ToByte(read.Invoke()); });
        }

		private static void RegisterIncrement(IVirtualMachine vm)
		{
            vm.RegisterCommand('+', b =>
            {
                if (vm.Memory[vm.MemoryPointer] > Byte.MaxValue - 1)
                    vm.Memory[vm.MemoryPointer] = 0;
                else
                    vm.Memory[vm.MemoryPointer]++;
            });
        }

		private static void RegisterDecrement(IVirtualMachine vm)
		{
            vm.RegisterCommand('-', b =>
            {
                if (vm.Memory[vm.MemoryPointer] <= byte.MinValue)
                    vm.Memory[vm.MemoryPointer] = Byte.MaxValue;
                else
                    vm.Memory[vm.MemoryPointer]--;
            });
        }

        private static void RegisterNext(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                if (vm.MemoryPointer >= vm.Memory.Length - 1)
                    vm.MemoryPointer = 0;
                else
                    vm.MemoryPointer++;
            });
        }

		private static void RegisterPrevious(IVirtualMachine vm)
		{
            vm.RegisterCommand('<', b =>
            {
                if (vm.MemoryPointer <= 0)
                    vm.MemoryPointer = vm.Memory.Length - 1;
                else
                    vm.MemoryPointer--;
            });
        }

        private static void RegisterLettersAndNumbers(char startSymbol, char endSymbol, IVirtualMachine vm)
		{
			for (char i = startSymbol; i <= endSymbol; i++)
			{
				char tempChar = i;
                vm.RegisterCommand(i, b => { vm.Memory[vm.MemoryPointer] = Convert.ToByte(tempChar); });
            }
		}
	}
}