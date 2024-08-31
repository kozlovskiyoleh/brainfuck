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
			vm.RegisterCommand('.', b => { write.Invoke(Convert.ToChar(b.MemoryPointer)); });
			vm.RegisterCommand('+', b => { vm.MemoryPointer++; });
			vm.RegisterCommand('-', b => { vm.MemoryPointer--; });
			vm.RegisterCommand(',', b => { b.MemoryPointer = read.Invoke(); });
			vm.RegisterCommand('>', b => { vm.MemoryPointer++; });
			vm.RegisterCommand('<', b => { vm.MemoryPointer--; });
			//...
		}
	}
}