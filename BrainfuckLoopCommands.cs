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
            int startIndex = 0;
            int count = 0;
            Stack<char> checkScopesParity = new Stack<char>();
			
            vm.RegisterCommand('[', b => 
            { 
                startIndex = vm.InstructionPointer; 
                count = vm.MemoryPointer;
                checkScopesParity.Push('[');
            });

			vm.RegisterCommand(']', b => 
            {
                if (vm.Memory[count] != 0) vm.InstructionPointer = startIndex;
                else if (checkScopesParity.Pop() != '[') throw new InvalidOperationException();
                //else if (checkScopesParity.Count == 0) throw new InvalidOperationException();
            });
		}

        public static bool VerifyLoop(string str)
        {
            Stack<char> stack = new Stack<char>();
            foreach (char symbol in str)
            {
                switch (symbol)
                {
                    case '[':
                        stack.Push(symbol);
                        break;
                    case ']':
                        if (stack.Count == 0) return false;
                        if (stack.Pop() != '[') return false;
                        break;
                }
            }
            return stack.Count == 0;
        }
    }
}