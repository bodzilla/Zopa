using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Common
{
    /// <inheritdoc />
    public class CsvStore : IDataStore
    {
        private readonly ILogger<CsvStore> _logger;
        private readonly string _path;

        public CsvStore(ILogger<CsvStore> logger, string path)
        {
            _logger = logger;
            _path = path;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> ExtractAll()
        {
            var lenders = new List<Lender>();
            try
            {
                using (var streamReader = new StreamReader(_path))
                {
                    using (var csvReader = new CsvReader(streamReader))
                    {
                        var records = csvReader.GetRecords<dynamic>();
                        lenders.AddRange(
                            records.Select(
                                lender => new Lender(lender.Lender, double.Parse(lender.Rate),
                                    double.Parse(lender.Available)))
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not extract data from CSV.", ex);
                throw;
            }
            return lenders;
        }
    }
}
