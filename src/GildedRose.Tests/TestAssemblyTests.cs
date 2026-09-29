using System.Collections.Generic;
using Xunit;

namespace GildedRose.Tests
{
    public class GildedRoseTests
    {
        [Fact]
        public void NormalItem_DegradesByOne_BeforeSellDate()
        {
            var items = new List<Item>
            {
                new Item { Name = "Normal Item", SellIn = 10, Quality = 20 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(9, items[0].SellIn);
            Assert.Equal(19, items[0].Quality);
        }

        [Fact]
        public void NormalItem_DegradesByTwo_AfterSellDate()
        {
            var items = new List<Item>
            {
                new Item { Name = "Normal Item", SellIn = 0, Quality = 20 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(-1, items[0].SellIn);
            Assert.Equal(18, items[0].Quality);
        }

        [Fact]
        public void Quality_NeverGoesBelowZero()
        {
            var items = new List<Item>
            {
                new Item { Name = "Normal Item", SellIn = 10, Quality = 0 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(0, items[0].Quality);
        }

        [Fact]
        public void AgedBrie_IncreasesQuality()
        {
            var items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 5, Quality = 10 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(11, items[0].Quality);
            Assert.Equal(4, items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_IncreasesTwiceAsFast_AfterSellDate()
        {
            var items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(12, items[0].Quality);
            Assert.Equal(-1, items[0].SellIn);
        }

        [Fact]
        public void AgedBrie_QualityNeverExceedsFifty()
        {
            var items = new List<Item>
            {
                new Item { Name = "Aged Brie", SellIn = 5, Quality = 50 }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(50, items[0].Quality);
        }

        [Fact]
        public void Sulfuras_NeverChanges()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Sulfuras, Hand of Ragnaros",
                    SellIn = 5,
                    Quality = 80
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(5, items[0].SellIn);
            Assert.Equal(80, items[0].Quality);
        }

        [Fact]
        public void BackstagePasses_IncreaseByOne_WhenMoreThanTenDays()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(21, items[0].Quality);
        }

        [Fact]
        public void BackstagePasses_IncreaseByTwo_WhenTenDaysOrLess()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 10,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(22, items[0].Quality);
        }

        [Fact]
        public void BackstagePasses_IncreaseByThree_WhenFiveDaysOrLess()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 5,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(23, items[0].Quality);
        }

        [Fact]
        public void BackstagePasses_DropToZero_AfterConcert()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 0,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(0, items[0].Quality);
        }

        [Fact]
        public void BackstagePasses_QualityNeverExceedsFifty()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 5,
                    Quality = 50
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(50, items[0].Quality);
        }

        [Fact]
        public void ConjuredItem_DegradesByTwo_BeforeSellDate()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Conjured Mana Cake",
                    SellIn = 10,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(9, items[0].SellIn);
            Assert.Equal(18, items[0].Quality);
        }

        [Fact]
        public void ConjuredItem_DegradesByFour_AfterSellDate()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Conjured Mana Cake",
                    SellIn = 0,
                    Quality = 20
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(-1, items[0].SellIn);
            Assert.Equal(16, items[0].Quality);
        }

        [Fact]
        public void ConjuredItem_QualityNeverGoesBelowZero()
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Conjured Mana Cake",
                    SellIn = 0,
                    Quality = 3
                }
            };

            var sut = new GildedRose(items);

            sut.UpdateQuality();

            Assert.Equal(0, items[0].Quality);
        }
    }
}