namespace Tessin.Tin.Models
{
    public enum TinStatus
    {
        /// <summary>
        /// The Tin is valid.
        /// </summary>
        Valid = 0,
        
        /// <summary>
        /// Validity could not be determined.
        /// </summary>
        Uncertain = 1,
        
        /// <summary>
        /// The Tin is invalid.
        /// </summary>
        Invalid = 2,
    }
}
