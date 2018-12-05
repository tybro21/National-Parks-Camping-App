using System;
using Capstone;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            CampingCLI cli = new CampingCLI();
            cli.RunCLI();
        }
    }
}
