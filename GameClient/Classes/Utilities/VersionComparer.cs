using System;

namespace GameClient.Classes.Utilities
{
    public class VersionComparer
    {
        #region Fields
        private readonly double _actualVersion;
        private readonly double _expectedVersion;
        #endregion


        #region Properties
        public VersionComparerResult Result { get; private set; }
        #endregion


        #region Constructor
        public VersionComparer(string actualVersion, string expectedVersion)
        {
            _actualVersion = GetComparableVersionNumber(actualVersion);
            _expectedVersion = GetComparableVersionNumber(expectedVersion);
            if (Result != VersionComparerResult.Invalid)
            {
                CompareVersions();
            }
        }
        #endregion


        #region Internal Implementation
        private void CompareVersions()
        {
            if (_actualVersion < _expectedVersion)
            {
                Result = VersionComparerResult.Outdated;
            }
            else if (_actualVersion > _expectedVersion)
            {
                Result = VersionComparerResult.Newer;
            }
            else //if (Math.Abs(_actualVersion - _expectedVersion) < Double.Epsilon)
            {
                Result = VersionComparerResult.Current;
            }
        }

        private double GetComparableVersionNumber(string versionNumber)
        {
            double result;
            if (!Double.TryParse(versionNumber, out result))
            {
                Result = VersionComparerResult.Invalid;
            }
            return result;
        }
        #endregion
    }
}