using System;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        covarDel del = Method1;
        Small sm1 = del(new Big());

        [Fact]
        public void Test1()
        {
        }
    }

    public delegate Small covarDel(Big mc);
    public class Small
    {

    }
    public class Big : Small
    {

    }
    public class Bigger : Big
    {

    }
}
