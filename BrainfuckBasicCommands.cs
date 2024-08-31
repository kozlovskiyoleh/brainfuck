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
		// ��� ������� Program ����� ����������� �� ASCII ������� ������� ������������ � byte � �������� � Memory
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