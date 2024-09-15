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
            List<Loop> nestedLoops = FindNestedLoops(vm.Instructions);
            int loopsCounter = 0;

            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = nestedLoops[loopsCounter].EndLoop;
                }
                loopsCounter++;
            });

            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                {
                    vm.InstructionPointer = nestedLoops[--loopsCounter].StartLoop;
                }
            });
        }

        private static List<Loop> FindNestedLoops(string instructions)
        {
            int countOpeneScopes = 0;
            List<Loop> tempList = new List<Loop>();
            for(int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i] == '[') 
                {
                    tempList.Add(new Loop(i-1));
                    countOpeneScopes++;
                }
                if (instructions[i] == ']')
                    tempList[--countOpeneScopes].AddEndLoopIndex(i);
            }
            return tempList;
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