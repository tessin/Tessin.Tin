using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Tessin.Tin.Models.Extensions;

namespace Tessin.Tin.Models
{
    public class TinResponse
    {
        
        /// <summary>
        /// Raw value of the Tin number.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Normalized value of the Tin number.
        /// </summary>
        public string NormalizedValue { get; set; }

        /// <summary>
        /// Refer to values in <see cref="TinCountry"/>
        /// </summary>
        public TinCountry Country { get; set; }

        /// <summary>
        /// Refer to values in <see cref="TinType"/>
        /// </summary>
        public TinType Type { get; set; }

        public TinEntityType EntityType { get; set; }

        /// <summary>
        /// Refer to values in <see cref="TinGender"/>
        /// </summary>
        public TinGender Gender { get; set; }

        /// <summary>
        /// Refer to values in <see cref="TinStatus"/>
        /// </summary>
        public TinStatus Status { get; set; }

        /// <summary>
        /// Date if any extracted from the Tin value.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Age of the person or entity if one could be calculated from Date.
        /// </summary>
        public int? Age { get; set; }

        public List<TinMessage> Messages { get; set; }

        public TinResponse()
        {
            Messages = new List<TinMessage>();
        }

        public static TinResponse ForError(TinMessageCode code, string value, TinCountry country)
        {
            var tin = new TinResponse { Country = country, Value = value, Status = TinStatus.Invalid };
            tin.Messages.Add(code.ToErrorMessage());
            return tin;
        }

        public override string ToString()
        {
            return $"{nameof(Value)} = {Value}+\r\n" +
                   $"{nameof(NormalizedValue)} = {NormalizedValue}\r\n" +
                   $"{nameof(Country)} = {Country.GetText()}\r\n" +
                   $"{nameof(Type)} = {Type.GetText()}\r\n" +
                   $"{nameof(EntityType)} = {EntityType}\r\n" +
                   $"{nameof(Gender)} = {Gender.GetText()}\r\n" +
                   $"{nameof(Status)} = {Status.GetText()}\r\n" +
                   $"{nameof(Date)} = {Date?.ToString("yyyy-MM-dd HH:mm:ss")}\r\n" +
                   $"{nameof(Age)} = {Age}\r\n" +
                   $"{nameof(Messages)} = {Messages.Select(p => p.ToString()).Aggregate((c, n) => c + n)}\r\n";
        }
    }
}