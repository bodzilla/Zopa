using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Zopa.Models;

namespace Zopa.Core.Common
{
    /// <summary>
    /// The class that manages data extraction.
    /// </summary>
    public class DataStore
    {
        private readonly ILogger<DataStore> _logger;
        private readonly string _path;

        public DataStore(ILogger<DataStore> logger, string path)
        {
            _logger = logger;
            _path = path;
        }

        /// <summary>
        /// Extracts all <see cref="Lender"/> from CSV.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lender> ExtractAllLenders()
        {
            var lenders = new List<Lender>();
            try
            {
                // Use CSV reader to extract records from fields.
                using (var streamReader = new StreamReader(_path))
                {
                    using (var csvReader = new CsvReader(streamReader))
                    {
                        var records = csvReader.GetRecords<dynamic>();
                        lenders.AddRange(
                            records.Select(
                                lender => new Lender(lender.Lender, double.Parse(lender.Rate), double.Parse(lender.Available)))
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not extract lenders from CSV.", ex);
                throw;
            }
            return lenders;
        }
    }
}
