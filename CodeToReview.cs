using System;
using System.Collections.Generic;//Typo error
using System.Linq;

namespace Utility.Valocity.ProfileHelper
{
    public class People
    {
        // REVIEW:
        // Use DateTimeOffset consistently instead of mixing DateTime and DateTimeOffset.
        // DateOfBirth is calculated when a default person is created.
        private static DateTimeOffset DefaultDateOfBirth =>
            DateTimeOffset.UtcNow.AddYears(-15);

        public string Name { get; }

        public DateTimeOffset DateOfBirth { get; }

        public People(string name)
            : this(name, DefaultDateOfBirth)//Refactoring constructer
        {
        }

        public People(string name, DateTimeOffset dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Name cannot be null or empty.",
                    nameof(name));
            }

            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }

    public class BirthingUnit
    {
        private const int MaxNameLength = 255;

        private readonly List<People> _people;
        private readonly Random _random;

        public BirthingUnit()
        {
            _people = new List<People>();
            _random = new Random();
        }

        /// <summary>
        /// Creates the specified number of people.
        /// </summary>
        /// <param name="count">Number of people to create.</param>
        /// <returns>The people created by this call.</returns>
        public IReadOnlyList<People> GetPeople(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Count cannot be negative.");
            }

            var peopleCreated = new List<People>();

            for (int i = 0; i < count; i++)
            {
                // REVIEW:
                // Random.Next(0, 1) always returns 0 because the upper
                // bound is exclusive. Use Next(2) to generate 0 or 1.
                string name = _random.Next(2) == 0
                    ? "Bob"
                    : "Betty";

                // REVIEW:
                // Don't approximate years using 356 days.
                // AddYears() correctly handles leap years.
                int age = _random.Next(18, 85);

                var dateOfBirth = DateTimeOffset.UtcNow.AddYears(-age);

                var person = new People(name, dateOfBirth);

                _people.Add(person);
                peopleCreated.Add(person);
            }

            // REVIEW:
            // Returning only the people created during this call avoids
            // surprising behavior caused by returning the entire accumulated list.
            return peopleCreated;
        }

        /// <summary>
        /// Gets Bob's records based on age criteria.
        /// </summary>
        /// <param name="olderThan30">
        /// If true, returns Bobs older than 30.
        /// Otherwise returns all Bobs.
        /// </param>
        private IEnumerable<People> GetBobs(bool olderThan30)
        {
            var bobs = _people.Where(
                person => string.Equals(
                    person.Name,
                    "Bob",
                    StringComparison.OrdinalIgnoreCase));

            if (!olderThan30)
            {
                return bobs;
            }

            // REVIEW:
            // Original code used:
            // DOB >= DateTime.Now.Subtract(30 years)
            //
            // That actually identifies people younger than approximately 30.
            // For older than 30, DOB must be BEFORE the 30-year cutoff.
            var cutoffDate = DateTimeOffset.UtcNow.AddYears(-30);

            return bobs.Where(person => person.DateOfBirth < cutoffDate);
        }

        /// <summary>
        /// Combines a person's first name with a last name.
        /// </summary>
        public string GetMarried(People person, string lastName)
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(
                    "Last name cannot be null or empty.",
                    nameof(lastName));
            }

            // REVIEW:
            // Original code checked "test" case-sensitively.
            // OrdinalIgnoreCase makes the intent explicit.
            if (lastName.Contains(
                    "test",
                    StringComparison.OrdinalIgnoreCase))
            {
                return person.Name;
            }

            string fullName = $"{person.Name} {lastName}";

            // REVIEW:
            // Original code called Substring() but ignored the result,
            // so nothing was actually truncated.
            if (fullName.Length > MaxNameLength)
            {
                fullName = fullName.Substring(0, MaxNameLength);
            }

            return fullName;
        }
    }
}