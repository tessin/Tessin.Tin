namespace Tessin.Tin.Models
{
    public enum TinGender
    {
        /// <summary>
        /// The gender cannot be determined but the Tin is for a person.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The Tin belongs to a man.
        /// </summary>
        Male = 1,
        /// <summary>
        /// The Tin belongs to a woman.
        /// </summary>
        Female = 2,
        /// <summary>
        /// Gender is not applicable to legal entities.
        /// </summary>
        Entity = 3
    }
}
