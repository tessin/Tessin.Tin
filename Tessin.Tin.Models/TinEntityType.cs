namespace Tessin.Tin.Models
{
    public class TinEntityType
    {

        /// <summary>
        /// Refer to ISO2 values in <see cref="TinCountry"/>
        /// </summary>
        public TinCountry Country { get; set; }

        /// <summary>
        /// Short name of the entity type (e.g. AB).
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Name of the entity type (e.g. Aktiebolag).
        /// </summary>
        public string Name { get; set; }

        public TinEntityType(TinCountry country, string name, string shortName)
        {
            Country = country;
            ShortName = shortName;
            Name = name;
        }

    }
}
