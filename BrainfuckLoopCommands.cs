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
            Dictionary<int, int> nestedLoops = FindNestedLoops(vm.Instructions);
            Stack<int> unmatchedScopes = new Stack<int>();

            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = nestedLoops[vm.InstructionPointer];
                }
                unmatchedScopes.Push(vm.InstructionPointer);
            });

            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                {
                    vm.InstructionPointer = nestedLoops[vm.InstructionPointer];
                }
            });
        }

        private static Dictionary<int, int> FindNestedLoops(string instructions)
        {
            List<int> openScopesIndexes = new();
            List<int> closedScopesIndexes = new();

            for(int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[') 
                    openScopesIndexes.Add(i);
                if (instructions[i] == ']')
                    closedScopesIndexes.Add(i);
            }
            closedScopesIndexes.Reverse();
            Dictionary<int, int> loopsScopesIndexes = new();
            for(int i = 0; i < openScopesIndexes.Count; i++)
            {
                loopsScopesIndexes.Add(openScopesIndexes[i], closedScopesIndexes[i]);
                loopsScopesIndexes.Add(closedScopesIndexes[i], openScopesIndexes[i]);
            }
            return loopsScopesIndexes;
        }
    }

    public class Loop
    {
        public int StartLoop { get; private set; }
        public int EndLoop { get; private set; }

        public Loop(int startIndex, int endIndex = 0)
        {
            StartLoop = startIndex;
            EndLoop = endIndex;
        }

        public void AddEndLoopIndex(int x) => EndLoop = x;
    }
}