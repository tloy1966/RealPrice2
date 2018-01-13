using System;
using NUnit.Framework;
using FluentAssertions;
namespace test_RealPrice
{
    [TestFixture]
    public class Getdata
    {
        [TestCase]
        public void TestMethod1()
        {
            int a = 1;
            a.Should().Be(1);
        }
    }
}
