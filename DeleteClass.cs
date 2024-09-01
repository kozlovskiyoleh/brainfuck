using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace func.brainfuck
{
        public class VirtualMachine3 : IVirtualMachine
        {
            public string Instructions { get; }
            public int InstructionPointer { get; set; }
            public byte[] Memory { get; } // напевно використовується для перетворення + в символ
            public int MemoryPointer { get; set; }
            public Dictionary<byte, Action<IVirtualMachine>> Register;

            public VirtualMachine3(string program, int memorySize)
            {
                Memory = new byte[memorySize];
                Instructions = program;
                Register = new Dictionary<byte, Action<IVirtualMachine>>();
                Memory = TransformInstructionsToByte(program, memorySize);
            }

            private byte[] TransformInstructionsToByte(string program, int memorySize)
            {
                byte[] tempMemory = new byte[memorySize];
                for (int i = 0; i < program.Length; i++)
                    tempMemory[i] = Convert.ToByte(program[i]);
                return tempMemory;
            }

            public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
            {
                Register.Add(Convert.ToByte(symbol), (IVirtualMachine) => execute(this));
            }

            public void Run()
            {
                while (MemoryPointer < Memory.Length)
                {
                    if (IsCommandInRegister())
                    {
                        var command = Memory[MemoryPointer];
                        Register[command](this);
                        MemoryPointer++;
                    }
                }
            }

            private bool IsCommandInRegister() => Register.ContainsKey(Memory[MemoryPointer]);
        }
}
