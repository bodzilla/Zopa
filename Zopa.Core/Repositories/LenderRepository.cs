using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.Extensions.Logging;
using Zopa.Core.Contracts;
using Zopa.Models;

namespace Zopa.Core.Repositories
{
    /// <inheritdoc />
    public class LenderRepository : IRepository<Lender>
    {
        private readonly ILogger<LenderRepository> _logger;
        private readonly string _path;

        public LenderRepository(ILogger<LenderRepository> logger, string path)
        {
            _logger = logger;
            _path = path;
        }

        /// <inheritdoc />
        public IEnumerable<Lender> GetAll()
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
