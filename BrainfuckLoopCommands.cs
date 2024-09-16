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
            int loopsCounter = 0;

            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = nestedLoops[vm.InstructionPointer];
                }
                loopsCounter++;
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
            int countOpeneScopes = 0;
            List<Loop> tempList = new List<Loop>();
            List<int> openScopesIndexes = new();
            for(int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[') 
                {
                    tempList.Add(new Loop(i));
                    countOpeneScopes++;
                }
                if (instructions[i] == ']')
                    tempList[--countOpeneScopes].AddEndLoopIndex(i);
            }
            Dictionary<int, int> loopsScopesIndexes = new();
            for(int i = 0; i < tempList.Count; i++)
            {
                loopsScopesIndexes.Add(tempList[i].StartLoop, tempList[i].EndLoop);
                loopsScopesIndexes.Add(tempList[i].EndLoop, tempList[i].StartLoop);
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