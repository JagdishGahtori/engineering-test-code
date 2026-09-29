using System.Collections.Generic;

namespace GildedRose.Console
{
    public class Program
    {
        public IList<Item> Items = new List<Item>();

        static void Main(string[] args)
        {
            System.Console.WriteLine("OMGHAI!");

            /*--------------------------------------------------------------------------*/
            /* Begin -- Clean code, easily undestood and using Strategy/Factory pattern.*/
            /*--------------------------------------------------------------------------*/

            var items = new List<Item>
            {
                new Item
                {
                    Name = "+5 Dexterity Vest",
                    SellIn = 10,
                    Quality = 20
                },
                new Item
                {
                    Name = "Aged Brie",
                    SellIn = 2,
                    Quality = 0
                },
                new Item
                {
                    Name = "Conjured Mana Cake",
                    SellIn = 3,
                    Quality = 6
                }
            };

            var app = new GildedRose(items);

            Console.WriteLine("Before Update");

            foreach (var item in app.Items)
            {
                Console.WriteLine(
                $"{item.Name}, SellIn = {item.SellIn}, Quality = {item.Quality}");
            }

            app.UpdateQuality();

            Console.WriteLine();
            Console.WriteLine("After Update");

            foreach (var item in app.Items)
            {
                Console.WriteLine(
                $"{item.Name}, SellIn = {item.SellIn}, Quality = {item.Quality}");
            }

            /*--------------------------------------------------------------------------*/
            /* End -- Clean code, easily undestood and using Strategy/Factory pattern.*/
            /*--------------------------------------------------------------------------*/
            
            System.Console.ReadKey();
        }
    }

    public class Item
    {
        public string Name { get; set; } = "";

        public int SellIn { get; set; }

        public int Quality { get; set; }
    }

    public class GildedRose
    {
        public IList<Item> Items { get; }

        public GildedRose(IList<Item> items)
        {
            Items = items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                ItemUpdaterFactory
                .Create(item)
                .Update(item);
            }
        }
    }

    public static class ItemUpdaterFactory
    {
        public static IItemUpdater Create(Item item)
        {
            switch (item.Name)
            {
                case "Aged Brie":
                    return new AgedBrieUpdater();

                case "Sulfuras, Hand of Ragnaros":
                    return new SulfurasUpdater();

                case "Backstage passes to a TAFKAL80ETC concert":
                    return new BackstagePassUpdater();

                default:
                    if (item.Name.StartsWith("Conjured"))
                    {
                        return new ConjuredUpdater();
                    }

                    return new NormalItemUpdater();
            }
        }
    }

    public interface IItemUpdater
    {
        void Update(Item item);
    }

    public abstract class ItemUpdaterBase : IItemUpdater
    {
        protected const int MaxQuality = 50;
        protected const int MinQuality = 0;

        public abstract void Update(Item item);

        protected void IncreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Min(MaxQuality, item.Quality + amount);
        }

        protected void DecreaseQuality(Item item, int amount = 1)
        {
            item.Quality = Math.Max(MinQuality, item.Quality - amount);
        }
    }

    public class NormalItemUpdater : ItemUpdaterBase
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            var degradation = item.SellIn < 0 ? 2 : 1;

            DecreaseQuality(item, degradation);
        }
    }

    public class AgedBrieUpdater : ItemUpdaterBase
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            IncreaseQuality(item);

            if (item.SellIn < 0)
            {
                IncreaseQuality(item);
            }
        }
    }

    public class SulfurasUpdater : ItemUpdaterBase
    {
        public override void Update(Item item)
        {
            // Legendary item. No changes.
        }
    }

    public class BackstagePassUpdater : ItemUpdaterBase
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            if (item.SellIn < 0)
            {
                item.Quality = 0;
                return;
            }

            IncreaseQuality(item);

            if (item.SellIn < 10)
            {
                IncreaseQuality(item);
            }

            if (item.SellIn < 5)
            {
                IncreaseQuality(item);
            }
        }
    }

    public class ConjuredUpdater : ItemUpdaterBase
    {
        public override void Update(Item item)
        {
            item.SellIn--;

            var degradation = item.SellIn < 0 ? 4 : 2;

            DecreaseQuality(item, degradation);
        }

    }
}


