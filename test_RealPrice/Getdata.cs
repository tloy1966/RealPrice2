using System;
using NUnit.Framework;
using FluentAssertions;
using RealPrice;
using RealPrice.Authority;
namespace test_RealPrice
{

    [TestFixture]
    public class Getdata
    {
        private RealPrice.Models.RealPriceContext _context = new RealPrice.Models.RealPriceContext();
        
        [Test]
        public void TestMethod1()
        {
            RealPrice.Models.RealPriceContext x;
            int a = 1;
            a.Should().Be(1);
        }
    }
}
