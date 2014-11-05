using System;
using System.Collections;
using System.Collections.Generic;

namespace Dapperism.Validation
{
    public class ValidationResults : IEnumerable<ValidationResult>
    {
        private readonly List<ValidationResult> _validationResults;

        public ValidationResults()
        {
            _validationResults = new List<ValidationResult>();
        }

        public IEnumerator<ValidationResult> GetEnumerator()
        {
            return _validationResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        internal void Add(ValidationResult validationResult)
        {
            _validationResults.Add(validationResult);
        }

    }
}
