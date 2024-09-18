using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        private static Dictionary<int, int> closedBracketsIndexes = new Dictionary<int, int>();
        private static Dictionary<int, int> openedBracketsindexes = new Dictionary<int, int>();

        public static void RegisterTo(IVirtualMachine vm)
        {
            InitScopesIndexes(vm.Instructions);
            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = closedBracketsIndexes[vm.InstructionPointer];
                }
            });

            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                {
                    vm.InstructionPointer = openedBracketsindexes[vm.InstructionPointer];
                }
            });
        }

        private static void InitScopesIndexes(string instructions)
        {
            Stack<int> stack = new Stack<int>();
            for(int i = 0; i < instructions.Length; i++)
            {
                char bracket = instructions[i];
                switch (bracket)
                {
                    case '[':
                        stack.Push(i);
                        break;
                    case ']':
                        openedBracketsindexes[i] = stack.Peek();
                        closedBracketsIndexes[stack.Pop()] = i;
                        break;
                }
            }           
        }
    }
}