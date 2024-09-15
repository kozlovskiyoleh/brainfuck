using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
            
            Stack<int> iterationIndexes = new Stack<int>();
            Stack<int> startIndexes = new Stack<int>(); 
			Stack<char> validScopes = new Stack<char>();
            vm.RegisterCommand('[', b => 
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = vm.Instructions.IndexOf(']', vm.InstructionPointer);
                    return;
                }
                startIndexes.Push(vm.InstructionPointer); 
                iterationIndexes.Push(vm.MemoryPointer);
                validScopes.Push('[');
            });

			vm.RegisterCommand(']', b => 
            {
                int countIterations = vm.Memory[iterationIndexes.Peek()];
                if (countIterations == 0)
                {
                    startIndexes.Pop();
                    iterationIndexes.Pop();
                    validScopes.Pop();
                }
                else
                {
                    vm.InstructionPointer = startIndexes.Peek();
                }
                
            });
		}
    }
}