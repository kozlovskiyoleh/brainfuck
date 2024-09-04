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
		// при отримані Program через конструктор всі ASCII символи потрібно конвертувати в byte і зберегти в Memory
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
			vm.RegisterCommand('>', b => { vm.MemoryPointer++; });
			vm.RegisterCommand('<', b => { vm.MemoryPointer--; });
			//...
		}
	}
}